using System;
using System.Buffers;
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

        public MCConnectionHandler(ILogger<MCConnectionHandler> logger)
        {
            _logger = logger;
        }

        public override Task OnConnectedAsync(ConnectionContext connection) 
            => HandleConnection(new MCConnectionContext(connection));

        private async Task HandleConnection(MCConnectionContext ctx)
        {
            while (ctx.Transport.Input.TryRead(out var readResult))
            {
                var buffer = readResult.Buffer;
                HandlePacket(buffer, ctx);
            }
        }

        private void HandlePacket(in ReadOnlySequence<byte> buffer, MCConnectionContext ctx)
        {
            var reader = new MCPacketReader(buffer);
            var lenght = reader.ReadVarInt();
            reader = new MCPacketReader(reader.Buffer.Slice(0, lenght));
            var id = reader.ReadVarInt();

            switch (ctx.ConnectionState)
            {
                case MCConnectionState.Handshaking when id is 0x00:
                    var protocolVersion = reader.ReadVarInt();
                    var serverAddress = reader.ReadString();
                    var port = reader.ReadUInt16();
                    var nextState = (MCConnectionState) reader.ReadVarInt();
                    _logger.LogInformation($"Received Successful Handshake Protocol: {(protocolVersion is PROTOCOL_VERSION ? "MATCH" : "ERROR")}; Address Used: {serverAddress}:{port}");
                    _logger.LogInformation($"Switching to {nextState}");
                    ctx.ConnectionState = nextState;
                    break;
                case MCConnectionState.Handshaking:
                    _logger.LogInformation($"Unknown Handshaking Packet {id:x2}");
                    break;
                case MCConnectionState.Status:
                    _logger.LogInformation($"Unknown Status Packet {id:x2}");
                    break;
                case MCConnectionState.Login:
                    _logger.LogInformation($"Unknown Login Packet {id:x2}");
                    break;
                case MCConnectionState.Playing:
                    _logger.LogInformation($"Unknown Playing Packet {id:x2}");
                    break;
                default:
                    _logger.LogCritical("Invalid Connection State. Aborting");
                    ctx.Abort();
                    break;
            }
        }
    }
}