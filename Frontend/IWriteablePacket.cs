using System;

namespace Frontend
{
    public interface IWriteablePacket
    {
        int Id { get; }

        int CalculateSize();

        void Write(IPacketWriter writer);
    }
}