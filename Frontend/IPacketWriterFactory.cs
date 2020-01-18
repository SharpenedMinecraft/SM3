using System;

namespace Frontend
{
    public interface IPacketWriterFactory
    {
        IPacketWriter CreateWriter(Memory<byte> memory);
    }
}