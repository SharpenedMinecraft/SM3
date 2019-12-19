using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;

namespace Frontend
{
    public sealed class MCConnectionHandler : ConnectionHandler
    {
        private const int PROTOCOL_VERSION = 498;
        private const string VERSION_NAME = "SM3-1.14.4";
        private ILogger _logger;
        private JsonSerializerOptions _jsonOptions;

        public MCConnectionHandler(ILogger<MCConnectionHandler> logger)
        {
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions()
            {
                IgnoreNullValues = true
            };
        }

        public override Task OnConnectedAsync(ConnectionContext connection) 
            => HandleConnection(new MCConnectionContext(connection));

        private async Task HandleConnection(MCConnectionContext ctx)
        {
            while (!ctx.ConnectionClosed.IsCancellationRequested)
            {
                var readResult = await ctx.Transport.Input.ReadAsync(ctx.ConnectionClosed);
                if (readResult.IsCanceled || readResult.IsCompleted)
                {
                    _logger.LogInformation("Connection Closed");
                    return;
                }
                
                var buffer = readResult.Buffer;
                HandlePacket(buffer, ctx);

                if (ctx.ShouldFlush)
                    await ctx.Transport.Output.FlushAsync();
                    
                if (ctx.ShouldClose /* we don't specifically close, we just hand it back to kestrel to deal with */)
                    return;
            }
        }

        private void HandlePacket(in ReadOnlySequence<byte> buffer, MCConnectionContext ctx)
        {
            var reader = new MCPacketReader(buffer);
            var writer = new MCPacketWriter(MemoryPool<byte>.Shared);
            var length = reader.ReadVarInt();

            if (length > reader.Buffer.Length || length < 1 /* 1 = small ID but no fields*/)
            {
                _logger.LogCritical($"Read Invalid length {length:X}. Aborting");
                ctx.Abort();
                return;
            }
            
            reader = new MCPacketReader(reader.Buffer.Slice(0, length));
            var id = reader.ReadVarInt();
            using var packetIdScope = _logger.BeginScope($"Packet ID: {id:x2}");

            switch (ctx.ConnectionState)
            {
                case MCConnectionState.Handshaking:
                    switch (id)
                    {
                        case 0x00:
                            var protocolVersion = reader.ReadVarInt();
                            var serverAddress = reader.ReadString();
                            var port = reader.ReadUInt16();
                            var nextState = (MCConnectionState) reader.ReadVarInt();
                            _logger.LogInformation($"Received Successful Handshake Protocol: {(protocolVersion is PROTOCOL_VERSION ? "MATCH" : "ERROR")}; Address Used: {serverAddress}:{port}");
                            _logger.LogInformation($"Switching to {nextState}");
                            ctx.ConnectionState = nextState;
                            break;
                        default:
                            _logger.LogInformation($"Unknown Handshaking Packet {id:x2}");
                            break;
                    }
                    break;
                case MCConnectionState.Status:
                    switch (id)
                    {
                        case 0x00: // Status Request
                            var payload = new StatusResponse(
                                new StatusResponse.VersionPayload(VERSION_NAME, PROTOCOL_VERSION),
                                new StatusResponse.PlayersPayload(100, 0, null), new ChatBuilder()
                                                                           .AppendText("This ")
                                                                           .WithColor("blue")
                                                                           .Bold()
                                                                           .WithExtra(builder => builder
                                                                                          .AppendText("is ")
                                                                                          .WithColor("red")
                                                                                          .Bold())
                                                                           .WithExtra(builder => builder
                                                                                          .AppendText("the ")
                                                                                          .WithColor("green")
                                                                                          .Bold())
                                                                           .WithExtra(builder => builder
                                                                                          .AppendText("MODT")
                                                                                          .WithColor("purple")
                                                                                          .Bold())
                                                                           .Build(),
                                null);
                            var payloadBytes = JsonSerializer.SerializeToUtf8Bytes(payload, _jsonOptions);
                            var dataSize1 = payloadBytes.Length + writer.GetVarIntSize(payloadBytes.Length) + writer.GetVarIntSize(0x00);
                            var packetSize1 = dataSize1 + writer.GetVarIntSize(dataSize1);
                            var span = ctx.Transport.Output.GetSpan(packetSize1);
                            writer.Span = span;
                            writer.WriteVarInt(dataSize1);
                            writer.WriteVarInt(0x00);
                            writer.WriteVarInt(payloadBytes.Length);
                            writer.WriteBytes(payloadBytes);
                            ctx.Transport.Output.Advance(packetSize1);
                            ctx.FlushNext();
                            break;
                        case 0x01: // Ping
                            var seed = reader.ReadInt64();
                            
                            var dataSize2 = writer.GetVarIntSize(0x01) + sizeof(long);
                            var packetSize2 = writer.GetVarIntSize(dataSize2) + dataSize2;
                            writer.Span = ctx.Transport.Output.GetSpan(packetSize2);
                            writer.WriteVarInt(dataSize2);
                            writer.WriteVarInt(0x01);
                            writer.WriteInt64(seed);
                            ctx.Transport.Output.Advance(packetSize2);
                            ctx.FlushNext();
                            break;
                        default:
                            _logger.LogInformation($"Unknown Status Packet {id:x2}");
                            break;
                    }
                    break;
                case MCConnectionState.Login:
                    switch (id)
                    {
                        case 0x00: // Login Start
                            var username = reader.ReadString();
                            _logger.LogInformation($"{username} trying to log in. Refusing.");

                            var payload = JsonSerializer.SerializeToUtf8Bytes(
                                new ChatBuilder()
                                    .AppendText("Currently not supporting Login\n\n")
                                    .Bold()
                                    .WithColor("red")
                                    .WithExtra(builder => builder
                                                   .Underlined()
                                                   .Italic()
                                                   .WithColor("green")
                                                   .AppendText("Powered by SM3"))
                                    .Build(), _jsonOptions);
                            var payloadSize = writer.GetVarIntSize(0x00) + writer.GetVarIntSize(payload.Length) +
                                             payload.Length;
                            var packetSize = payloadSize + writer.GetVarIntSize(payloadSize);
                            writer.Span = ctx.Transport.Output.GetSpan(packetSize);
                            writer.WriteVarInt(payloadSize);
                            writer.WriteVarInt(0x00);
                            writer.WriteVarInt(payload.Length);
                            writer.WriteBytes(payload);
                            ctx.Transport.Output.Advance(packetSize);
                            ctx.FlushNext();
                            break;
                        default:
                            _logger.LogInformation($"Unknown Login Packet {id:x2}");
                            break;
                    }
                    break;
                case MCConnectionState.Playing:
                    switch (id)
                    {
                        default:
                            _logger.LogInformation($"Unknown Playing Packet {id:x2}");
                            break;
                    }
                    break;
                default:
                    _logger.LogCritical("Invalid Connection State. Aborting");
                    ctx.Abort();
                    break;
            }
            // NOT IDEAL, but easiest
            ctx.Transport.Input.AdvanceTo(buffer.GetPosition(length + writer.GetVarIntSize(length)));
        }
    }
}