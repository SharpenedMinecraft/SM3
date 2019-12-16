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
        private string VERSION_NAME = "SM3-1.14.4";
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
                        default:
                            _logger.LogInformation($"Unknown Status Packet {id:x2}");
                            break;
                    }
                    break;
                case MCConnectionState.Login:
                    switch (id)
                    {
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