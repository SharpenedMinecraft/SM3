namespace Frontend.Packets.Play
{
    public readonly struct EntityStatus : IWriteablePacket
    {
        public int Id => 0x1C;

        public readonly int EntityId;
        public readonly byte Status;

        public EntityStatus(int entityId, byte status)
        {
            EntityId = entityId;
            Status = status;
        }

        public void Write(IPacketWriter writer)
        {
            writer.WriteInt32(EntityId);
            writer.WriteUInt8(Status);
        }
    }
}