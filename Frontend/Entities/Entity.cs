using System;
using System.Collections.Generic;
using System.Numerics;

namespace SM3.Frontend.Entities
{
    public abstract class Entity : IEntity
    {
        public abstract string Type { get; }
        public virtual int TypeId => EntityRegistry[Type];
        public abstract IEnumerable<IWriteablePacket> SpawnPackets { get; }

        protected IEntityRegistry EntityRegistry { get; }
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public int DimensionId { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 LookDir { get; set; } = Vector3.UnitX;

        /// <summary>
        /// The Entities Velocity in Block / Tick (50ms)
        /// 1 Block/Tick = 50ms
        /// </summary>
        public Vector3 Velocity { get; set; }
        public bool OnFire { get; set; }
        public bool Crouched { get; set; }
        // Unused (riding)
        public bool Sprinting { get; set; }
        public bool Swimming { get; set; }
        public bool Invisible { get; set; }
        public bool Glowing { get; set; }
        public bool FlyingElytra { get; set; }
        public int Air { get; set; } = 300;
        public Chat? CustomName { get; set; } = null;
        public bool IsCustomNameVisible { get; set; }
        public bool IsSilent { get; set; }
        public bool NoGravity { get; set; }
        public Pose Pose { get; set; }

        protected Entity(IEntityRegistry entityRegistry)
        {
            EntityRegistry = entityRegistry;
        }

        public virtual void ProcessTick(IEntityManager preTick, IEntityManager postTick)
        {
            ref var pre = ref preTick.GetEntity<Entity>(Id);
            ref var post = ref postTick.GetEntity<Entity>(Id);
            post.Position = pre.Position + pre.Velocity;
        }

        public virtual void WriteMetadata(EntityMetadata metadata)
        {
            metadata.WriteUInt8(0,
                (byte) ((OnFire ? 0x01 : 0x00) | (Crouched ? 0x02 : 0x00) | (Sprinting ? 0x08 : 0x00) |
                        (Swimming ? 0x010 : 0x00) | (Invisible ? 0x20 : 0x00) | (Glowing ? 0x40 : 0x00) |
                        (FlyingElytra ? 0x80 : 0x00)));
            metadata.WriteVarInt(1, Air);
            metadata.WriteOptChat(2, CustomName);
            metadata.WriteBoolean(3, IsCustomNameVisible);
            metadata.WriteBoolean(4, IsSilent);
            metadata.WriteBoolean(5, NoGravity);
            metadata.WritePose(6, Pose);
        }
    }

    public enum Pose : int
    {
        Standing = 0,
        FallFlying = 1,
        Sleeping = 2,
        Swimming = 3,
        SpinAttack = 4,
        Sneaking = 5,
        Dying = 6
    }
}
