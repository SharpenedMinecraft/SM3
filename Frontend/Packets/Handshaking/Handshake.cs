using System;
using App.Metrics.Logging;
using EnumsNET;
using Frontend.Packets.Login;
using Messaging;
using Messaging.Messages;
using Microsoft.Extensions.Logging;

namespace Frontend.Packets.Handshaking
{
    public struct Handshake : IReadablePacket
    {
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

        public bool Validate(ILogger logger)
        {
            if (ProtocolVersion != MCPacketHandler.ProtocolVersion)
            {
                logger.LogCritical($"Version missmatch. Server: {MCPacketHandler.ProtocolVersion} ({MCPacketHandler.VersionName}), Client: {ProtocolVersion}");
                return false;
            }

            if (NextStage != MCConnectionStage.Login || NextStage != MCConnectionStage.Status)
            {
                logger.LogCritical($"Invalid Next Stage {NextStage.AsString()}");
                return false;
            }

            return true;
        }

        public void UpdateState(IConnectionState state)
        {
            state.ConnectionStage = NextStage;
        }

        public void Message(IMessagingProvider messagingProvider)
        {
            messagingProvider.OnClientHandshake(new ClientHandshake(ProtocolVersion, ServerAddress, Port, NextStage == MCConnectionStage.Login));
        }
    }
}