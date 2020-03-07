using System.Buffers;
using SM3.Network;

namespace SM3.Frontend
{
    public sealed class MCPacketReaderFactory : IPacketReaderFactory
    {
        public IPacketReader CreateReader(ReadOnlySequence<byte> buffer)
        {
            return new MCPacketReader(buffer);
        }
    }
}