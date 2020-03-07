namespace SM3.Network.Play
{
    public readonly struct EntityStatus : IWriteablePacket
    {
        public int Id => 0x1C;

        public readonly Entity Entity;
        public readonly byte Status;

        public EntityStatus(Entity entity, byte status)
        {
            Entity = entity;
            Status = status;
        }

        public void Write(IPacketWriter writer)
        {
            writer.WriteInt32(Entity.NumericId);
            writer.WriteUInt8(Status);
        }
    }
}