using System.Collections.Generic;
using SM3.Frontend.Packets.Play;

namespace SM3.Frontend.Entities
{
    public abstract class MobEntity : Entity
    {
        public override IEnumerable<IWriteablePacket> SpawnPackets
        {
            get
            {
                yield return new SpawnMob(this);
                yield return new Packets.Play.EntityMetadata(this);
            }
        }

        protected MobEntity(IEntityRegistry entityRegistry) : base(entityRegistry)
        {
        }
    }
}
