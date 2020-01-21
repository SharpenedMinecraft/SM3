using Frontend.Entities;

namespace Frontend.Packets.Play
{
    public readonly struct EntityMetadata : IWriteablePacket
    {
        public int Id => 0x44;

        public readonly Entity Entity;

        public EntityMetadata(Entity entity)
        {
            Entity = entity;
        }

        public void Write(IPacketWriter writer)
        {
            writer.WriteVarInt(Entity.Id);
            var metadata = new Frontend.EntityMetadata(writer);
            Entity.WriteMetadata(metadata);
            metadata.WriteEnd();
        }
    }
}
