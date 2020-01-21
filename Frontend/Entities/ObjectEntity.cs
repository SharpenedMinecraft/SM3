using System.Collections.Generic;
using Frontend.Packets.Play;

namespace Frontend.Entities
{
    public abstract class ObjectEntity : Entity
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

        protected ObjectEntity(IEntityRegistry entityRegistry) : base(entityRegistry)
        { }
    }
}