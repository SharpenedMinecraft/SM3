using System;

namespace SM3.Frontend
{
    public interface IPacketWriterFactory
    {
        IPacketWriter CreateWriter(Memory<byte> memory);
    }
}