using System;

namespace Frontend
{
    public interface IPacketWriterFactory
    {
        public IPacketWriter CreateWriter(Memory<byte> memory);
    }
}