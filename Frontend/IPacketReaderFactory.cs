using System.Buffers;

namespace Frontend
{
    public interface IPacketReaderFactory
    {
        IPacketReader CreateReader(ReadOnlySequence<byte> buffer);
    }
}