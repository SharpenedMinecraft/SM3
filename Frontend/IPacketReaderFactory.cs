using System.Buffers;

namespace SM3.Frontend
{
    public interface IPacketReaderFactory
    {
        IPacketReader CreateReader(ReadOnlySequence<byte> buffer);
    }
}