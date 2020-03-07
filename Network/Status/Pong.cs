namespace SM3.Network.Status
{
    public readonly struct Pong : IWriteablePacket
    {
        public int Id => 0x01;

        public readonly long Seed;

        public Pong(long seed)
        {
            Seed = seed;
        }

        public void Write(IPacketWriter writer)
        {
            writer.WriteInt64(Seed);
        }
    }
}