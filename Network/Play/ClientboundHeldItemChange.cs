namespace SM3.Network.Play
{
    public readonly struct ClientboundHeldItemChange : IWriteablePacket
    {
        public int Id => 0x40;

        public readonly byte Slot;

        public ClientboundHeldItemChange(byte slot)
        {
            Slot = slot;
        }

        public void Write(IPacketWriter writer)
        {
            writer.WriteUInt8(Slot);
        }
    }
}