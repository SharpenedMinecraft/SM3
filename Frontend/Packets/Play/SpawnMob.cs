using System;
using Frontend.Entities;

namespace Frontend.Packets.Play
{
    public readonly struct SpawnMob : IWriteablePacket
    {
        public int Id => 0x03;

        public readonly MobEntity Entity;

        public SpawnMob(MobEntity entity)
        {
            Entity = entity;
        }

        public void Write(IPacketWriter writer)
        {
            writer.WriteVarInt(Entity.Id);
            writer.WriteGuid(Entity.Guid);
            writer.WriteVarInt(Entity.TypeId);
            writer.WriteDouble(Entity.Position.X);
            writer.WriteDouble(Entity.Position.Y);
            writer.WriteDouble(Entity.Position.Z);

            var rotation = RotationHelper.FromLookAt(Entity.LookDir);
            var pitch = rotation.X;
            var yaw = rotation.Y;
            writer.WriteUInt8(RotationHelper.RadiansTo256Angle(yaw));
            writer.WriteUInt8(RotationHelper.RadiansTo256Angle(pitch));
            writer.WriteUInt8(0);

            writer.WriteInt16((short)MathF.Round(Entity.Velocity.X / 8000));
            writer.WriteInt16((short)MathF.Round(Entity.Velocity.Y / 8000));
            writer.WriteInt16((short)MathF.Round(Entity.Velocity.Z / 8000));
        }
    }
}
