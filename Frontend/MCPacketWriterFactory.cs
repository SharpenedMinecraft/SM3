using System;
using SM3.Network;

namespace SM3.Frontend
{
    public sealed class MCPacketWriterFactory : IPacketWriterFactory
    {
        public IPacketWriter CreateWriter(Memory<byte> memory)
        {
            return new MCPacketWriter(memory);
        }
    }
}