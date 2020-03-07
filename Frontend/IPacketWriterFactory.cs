using System;
using SM3.Network;

namespace SM3.Frontend
{
    public interface IPacketWriterFactory
    {
        IPacketWriter CreateWriter(Memory<byte> memory);
    }
}