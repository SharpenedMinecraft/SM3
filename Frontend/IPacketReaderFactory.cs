using System.Buffers;

namespace Frontend
{
    public interface IPacketReaderFactory
    {
        public IPacketReader CreateReader(ReadOnlySequence<byte> buffer);
    }
}