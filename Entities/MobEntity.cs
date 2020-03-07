﻿using System.Collections.Generic;
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
                yield return new SpawnMob(Entity);
                yield return new Network.Play.EntityMetadata(Entity);
            }
        }

        protected MobEntity(Entity entity, IEntityRegistry entityRegistry) : base(entity, entityRegistry)
        {
        }
    }
}
