using System;

namespace Frontend
{
    public interface IWriteablePacket
    {
        int Id { get; }

        void Write(IPacketWriter writer);
    }
}