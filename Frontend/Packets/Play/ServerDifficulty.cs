namespace Frontend.Packets.Play
{
    public readonly struct ServerDifficulty : IWriteablePacket
    {
        public int Id => 0x0D;

        public readonly byte Difficulty;
        public readonly bool IsLocked;

        public ServerDifficulty(byte difficulty, bool isLocked)
        {
            Difficulty = difficulty;
            IsLocked = isLocked;
        }

        public int CalculateSize() => sizeof(byte) + sizeof(bool);

        public void Write(IPacketWriter writer)
        {
            writer.WriteUInt8(Difficulty);
            writer.WriteBoolean(IsLocked);
        }
    }
}