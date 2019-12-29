using Microsoft.Extensions.Logging;

namespace Frontend.Packets.Status
{
    public struct Pong : IWriteablePacket
    {
        public readonly int Id => 0x01;

        public long Seed;

        public Pong(long seed)
        {
            Seed = seed;
        }

        public int CalculateSize() => sizeof(long);

        public readonly void Write(IPacketWriter writer)
        {
            writer.WriteInt64(Seed);
        }
    }
}