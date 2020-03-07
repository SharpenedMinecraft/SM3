using System.Collections.Generic;
using SM3.Network;
using SM3.Network.Play;

namespace SM3.Entities
{
    public abstract class MobEntity : BaseEntity
    {
        public override IEnumerable<IWriteablePacket> SpawnPackets
        {
            get
            {
                yield return new SpawnMob(this);
                yield return new Network.Play.EntityMetadata(this);
            }
        }

        protected MobEntity(Entity entity, IEntityRegistry entityRegistry) : base(entity, entityRegistry)
        {
        }
    }
}
