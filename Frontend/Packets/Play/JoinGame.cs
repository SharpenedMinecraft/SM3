namespace SM3.Frontend.Packets.Play
{
    public readonly struct JoinGame : IWriteablePacket
    {
        public int Id => 0x26;

        public readonly int EntityId;
        public readonly byte GameMode;
        public readonly int Dimension;
        public readonly long Seed;
        public readonly byte MaxPlayers;
        public readonly string LevelType;
        public readonly int ViewDistance;
        public readonly bool ReducedDebugInfo;
        public readonly bool ShowDeathScreen;

        public JoinGame(int entityId, byte gameMode, int dimension, long seed, byte maxPlayers, string levelType, int viewDistance, bool reducedDebugInfo, bool showDeathScreen)
        {
            EntityId = entityId;
            GameMode = gameMode;
            Dimension = dimension;
            Seed = seed;
            MaxPlayers = maxPlayers;
            LevelType = levelType;
            ViewDistance = viewDistance;
            ReducedDebugInfo = reducedDebugInfo;
            ShowDeathScreen = showDeathScreen;
        }

        public void Write(IPacketWriter writer)
        {
            writer.WriteInt32(EntityId);
            writer.WriteUInt8(GameMode);
            writer.WriteInt32(Dimension);
            writer.WriteInt64(Seed);
            writer.WriteUInt8(MaxPlayers);
            writer.WriteString(LevelType);
            writer.WriteVarInt(ViewDistance);
            writer.WriteBoolean(ReducedDebugInfo);
            writer.WriteBoolean(!ShowDeathScreen);
        }
    }
}