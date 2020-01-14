using System;
using EnumsNET;
using Microsoft.Extensions.Logging;

namespace Frontend.Packets.Handshaking
{
    public struct Handshake : IReadablePacket
    {
        public readonly int Id => 0x00;
        public readonly MCConnectionStage Stage => MCConnectionStage.Handshaking;

        public int ProtocolVersion;
        public string ServerAddress;
        public short Port;
        public MCConnectionStage NextStage;

        public void Read(IPacketReader reader)
        {
            ProtocolVersion = reader.ReadVarInt();
            ServerAddress = reader.ReadString().ToString();
            Port = reader.ReadInt16();
            NextStage = (MCConnectionStage) reader.ReadVarInt();
        }

        public readonly void Process(ILogger logger, IConnectionState connectionState, IServiceProvider serviceProvider)
        {
            connectionState.ConnectionStage = NextStage;
            connectionState.IsLocal = ServerAddress == "localhost" || ServerAddress == "127.0.0.1";

            logger.LogInformation($"Received Handshake; Protocol {(ProtocolVersion is MCPacketHandler.ProtocolVersion ? "MATCH" : "ERROR")}; Address Used: {ServerAddress}:{Port}");
            logger.LogInformation($"Switching to {NextStage.AsString()}");
        }
    }
}