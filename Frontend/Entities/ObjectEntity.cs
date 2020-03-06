using System.Collections.Generic;
using SM3.Frontend.Packets.Play;

namespace SM3.Frontend.Entities
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
