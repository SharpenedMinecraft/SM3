using System;

namespace Frontend.Entities
{
    public abstract class ItemedThrowable : Throwable
    {
        public NetworkSlot Item { get; } = default; // empty, default(bool) == false = Present

        protected ItemedThrowable(IEntityRegistry entityRegistry) : base(entityRegistry)
        { }

        public override void WriteMetadata(EntityMetadata metadata)
        {
            base.WriteMetadata(metadata);
            metadata.WriteSlot(7, Item);
        }
    }
}