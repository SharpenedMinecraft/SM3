using System.Collections.Generic;
using Frontend.Packets.Play;

namespace Frontend.Entities
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