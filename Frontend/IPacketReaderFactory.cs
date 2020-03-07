using System.Buffers;
using SM3.Network;

namespace SM3.Frontend
{
    public interface IPacketReaderFactory
    {
        IPacketReader CreateReader(ReadOnlySequence<byte> buffer);
    }
}