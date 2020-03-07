namespace SM3.Network.Play
{
    public readonly struct EntityMetadata : IWriteablePacket
    {
        public int Id => 0x44;

        public readonly INetworkEntity Entity;

        public EntityMetadata(INetworkEntity entity)
        {
            Entity = entity;
        }

        public void Write(IPacketWriter writer)
        {
            writer.WriteVarInt(Entity.Entity.NumericId);
            var metadata = new Network.EntityMetadata(writer);
            Entity.WriteMetadata(metadata);
            metadata.WriteEnd();
        }
    }
}