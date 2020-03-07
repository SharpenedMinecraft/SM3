namespace SM3.Network.Play
{
    public readonly struct EntityIdle : IWriteablePacket
    {
        public int Id => 0x2C;

        public readonly Entity Entity;

        public EntityIdle(Entity entity)
        {
            Entity = entity;
        }

        public void Write(IPacketWriter writer)
        {
            writer.WriteVarInt(Entity.NumericId);
        }
    }
}
