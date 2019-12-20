using System;
using System.Buffers;

namespace Frontend
{
    public sealed class MCPacketWriterFactory : IPacketWriterFactory
    {
        public IPacketWriter CreateWriter(Memory<byte> memory)
        {
            return new MCPacketWriter(memory, MemoryPool<byte>.Shared);
        }
    }
}