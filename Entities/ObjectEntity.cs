using System.Collections.Generic;
using SM3.Network;
using SM3.Network.Play;

namespace SM3.Entities
{
    public abstract class ObjectEntity : BaseEntity
    {
        public override IEnumerable<IWriteablePacket> SpawnPackets
        {
            get
            {
                yield return new EntityIdle(Entity);
                yield return new SpawnObject(Entity);
                yield return new Network.Play.EntityMetadata(Entity);
            }
        }

        protected ObjectEntity(Entity entity, IEntityRegistry entityRegistry) : base(entity, entityRegistry)
        {
        }
    }
}
