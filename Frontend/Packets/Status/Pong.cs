using Microsoft.Extensions.Logging;

namespace Frontend.Packets.Status
{
    public struct Pong : IPacket
    {
        public readonly int Id => 0x01;
        public readonly MCConnectionStage Stage => MCConnectionStage.Status;
        public readonly bool IsServerbound => false;
        public readonly int Size => sizeof(long);

        public long Seed;

        public Pong(long seed)
        {
            Seed = seed;
        }

        public readonly void Write(IPacketWriter writer)
        {
            writer.WriteInt64(Seed);
        }

        public void Read(IPacketReader reader)
        {
            Seed = reader.ReadInt64();
        }

        public readonly void Process(ILogger logger, IConnectionState connectionState, IPacketQueue packetQueue)
        {
            // nothing
        }
    }
}