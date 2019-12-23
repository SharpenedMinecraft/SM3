using System;
using EnumsNET;
using Microsoft.Extensions.Logging;

namespace Frontend.Packets.Serverbound.Handshaking
{
    public struct Handshake : IPacket
    {
        public readonly int Id => 0x00;
        public readonly MCConnectionStage Stage => MCConnectionStage.Handshaking;
        public readonly bool IsServerbound => true;

        public int Size =>
            _size ??= MCPacketWriter.GetVarIntSize(ProtocolVersion)
                    + MCPacketWriter.GetVarIntSize(ServerAddress.Length)
                    + ServerAddress.Length
                    + sizeof(short)
                    + MCPacketWriter.GetVarIntSize((int) NextStage);

        public int ProtocolVersion;
        public string ServerAddress;
        public short Port;
        public MCConnectionStage NextStage;

        private int? _size;

        public readonly void Write(IPacketWriter writer)
        {
            writer.WriteVarInt(Id);
            writer.WriteVarInt(ProtocolVersion);
            writer.WriteString(ServerAddress);
            writer.WriteInt16(Port);
            writer.WriteVarInt((int)NextStage);
        }

        public void Read(IPacketReader reader)
        {
            ProtocolVersion = reader.ReadVarInt();
            ServerAddress = reader.ReadString().ToString();
            Port = reader.ReadInt16();
            NextStage = (MCConnectionStage) reader.ReadVarInt();
        }

        public readonly void Process(ILogger logger, IConnectionState connectionState, IPacketQueue packetQueue)
        {
            connectionState.ConnectionStage = NextStage;
            logger.LogInformation($"Received Handshake; Protocol {(ProtocolVersion is MCPacketHandler.PROTOCOL_VERSION ? "MATCH" : "ERROR")}; Address Used: {ServerAddress}:{Port}");
            logger.LogInformation($"Switching to {NextStage.AsString()}");
        }
    }
}