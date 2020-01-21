namespace Frontend.Packets.Play
{
    public readonly struct EntityIdle : IWriteablePacket
    {
        public int Id => 0x2C;

        public readonly IEntity Entity;

        public EntityIdle(IEntity entity)
        {
            Entity = entity;
        }

        public void Write(IPacketWriter writer)
        {
            writer.WriteVarInt(Entity.Id);
        }
    }
}
