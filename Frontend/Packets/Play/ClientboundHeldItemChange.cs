using System;

namespace Frontend.Packets.Play
{
    public readonly struct ClientboundHeldItemChange : IWriteablePacket
    {
        public int Id => 0x3F;

        public readonly byte Slot;

        public ClientboundHeldItemChange(byte slot)
        {
            Slot = slot;
        }

        public int CalculateSize() => sizeof(byte);

        public void Write(IPacketWriter writer)
        {
            writer.WriteUInt8(Slot);
        }
    }
}