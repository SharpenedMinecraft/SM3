using System.Buffers;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Frontend
{
    public sealed class MCPacketHandler : IPacketHandler
    {
        private readonly ILogger _logger;
        private const int PROTOCOL_VERSION = 498;
        private const string VERSION_NAME = "SM3-1.14.4";
        private JsonSerializerOptions _jsonOptions;
        
        public MCPacketHandler(ILogger<MCPacketHandler> logger)
        {
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions()
            {
                IgnoreNullValues = true
            };
        }
        
        public void HandlePacket(MCConnectionContext ctx, IPacketReader reader, IPacketWriterFactory writerFactory, int id)
        {
            IPacketWriter writer;
            switch (ctx.ConnectionState)
            {
                case MCConnectionState.Handshaking:
                    switch (id)
                    {
                        case 0x00:
                            var protocolVersion = reader.ReadVarInt();
                            var serverAddress = reader.ReadString().ToString();
                            ctx.IsLocal = serverAddress == "localhost" || serverAddress == "127.0.0.1";
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
                            var dataSize1 = payloadBytes.Length + MCPacketWriter.GetVarIntSize(payloadBytes.Length) + MCPacketWriter.GetVarIntSize(0x00);
                            var packetSize1 = dataSize1 + MCPacketWriter.GetVarIntSize(dataSize1);
                            var memory = ctx.Transport.Output.GetMemory(packetSize1);
                            writer = writerFactory.CreateWriter(memory);
                            writer.WriteVarInt(dataSize1);
                            writer.WriteVarInt(0x00);
                            writer.WriteVarInt(payloadBytes.Length);
                            writer.WriteBytes(payloadBytes);
                            ctx.Transport.Output.Advance(packetSize1);
                            ctx.FlushNext();
                            break;
                        case 0x01: // Ping
                            var seed = reader.ReadInt64();
                            
                            var dataSize2 = MCPacketWriter.GetVarIntSize(0x01) + sizeof(long);
                            var packetSize2 = MCPacketWriter.GetVarIntSize(dataSize2) + dataSize2;
                            writer = writerFactory.CreateWriter(ctx.Transport.Output.GetMemory(packetSize2));
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
                            var username = reader.ReadString().ToString();

                            if (!ctx.IsLocal)
                            {
                                _logger.LogInformation($"{username} trying to log in from Remote. Refusing.");

                                var payload = JsonSerializer.SerializeToUtf8Bytes(
                                    new ChatBuilder()
                                        .AppendText("Connection from Remote is not supported\n\n")
                                        .Bold()
                                        .WithColor("red")
                                        .Build(), _jsonOptions);
                                var payloadSize = MCPacketWriter.GetVarIntSize(0x00) +
                                                  MCPacketWriter.GetVarIntSize(payload.Length) +
                                                  payload.Length;
                                var packetSize = payloadSize + MCPacketWriter.GetVarIntSize(payloadSize);
                                writer = writerFactory.CreateWriter(ctx.Transport.Output.GetMemory(packetSize));
                                writer.WriteVarInt(payloadSize);
                                writer.WriteVarInt(0x00);
                                writer.WriteVarInt(payload.Length);
                                writer.WriteBytes(payload);
                                ctx.Transport.Output.Advance(packetSize);
                                ctx.FlushNext();
                            }

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
        }
    }
}