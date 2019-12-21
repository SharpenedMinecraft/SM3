using System.Buffers;

namespace Frontend
{
    public sealed class MCPacketReaderFactory : IPacketReaderFactory
    {
        public IPacketReader CreateReader(ReadOnlySequence<byte> buffer)
        {
            return new MCPacketReader(buffer);
        }
    }
}