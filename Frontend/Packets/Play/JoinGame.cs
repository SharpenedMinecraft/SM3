using System;
using Microsoft.Extensions.Logging;

namespace Frontend.Packets.Play
{
    public readonly struct JoinGame : IWriteablePacket
    {
        public int Id => 0x26;

        public readonly int EntityId;
        public readonly byte GameMode;
        public readonly int Dimension;
        public readonly byte MaxPlayers;
        public readonly string LevelType;
        public readonly int ViewDistance;
        public readonly bool ReducedDebugInfo;

        public JoinGame(int entityId, byte gameMode, int dimension, byte maxPlayers, string levelType, int viewDistance, bool reducedDebugInfo)
        {
            EntityId = entityId;
            GameMode = gameMode;
            Dimension = dimension;
            MaxPlayers = maxPlayers;
            LevelType = levelType;
            ViewDistance = viewDistance;
            ReducedDebugInfo = reducedDebugInfo;
        }

        public void Write(IPacketWriter writer)
        {
            writer.WriteInt32(EntityId);
            writer.WriteUInt8(GameMode);
            writer.WriteInt32(Dimension);
            writer.WriteUInt8(MaxPlayers);
            writer.WriteString(LevelType);
            writer.WriteVarInt(ViewDistance);
            writer.WriteBoolean(ReducedDebugInfo);
        }
    }
}