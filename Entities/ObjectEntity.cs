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
                yield return new EntityIdle(this);
                yield return new SpawnObject(this);
                yield return new Packets.Play.EntityMetadata(this);
            }
        }

        protected ObjectEntity(Entity entity, IEntityRegistry entityRegistry) : base(entity, entityRegistry)
        {
        }
    }
}
