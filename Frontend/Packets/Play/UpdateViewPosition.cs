namespace Frontend.Packets.Play
{
    public readonly struct UpdateViewPosition : IWriteablePacket
    {
        public int Id => 0x41;

        public readonly int ChunkX;
        public readonly int ChunkY;

        public UpdateViewPosition(int chunkX, int chunkY)
        {
            ChunkX = chunkX;
            ChunkY = chunkY;
        }

        public void Write(IPacketWriter writer)
        {
            writer.WriteVarInt(ChunkX);
            writer.WriteVarInt(ChunkY);
        }
    }
}