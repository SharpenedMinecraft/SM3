using SM3.Network;
using SM3.Network.Play;
using EntityMetadata = SM3.Network.EntityMetadata;

namespace SM3.Entities
{
    public abstract class ItemedThrowable : Throwable
    {
        public NetworkSlot Item { get; } = default; // empty, default(bool) == false = Present

        public override void WriteMetadata(EntityMetadata metadata)
        {
            base.WriteMetadata(metadata);
            metadata.WriteSlot(7, Item);
        }

        protected ItemedThrowable(Entity entity, IEntityRegistry entityRegistry) : base(entity, entityRegistry)
        { }
    }
}
