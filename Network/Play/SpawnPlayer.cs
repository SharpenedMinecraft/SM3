using System;

namespace SM3.Network.Play
{
    public readonly struct SpawnPlayer : IWriteablePacket
    {
        public int Id => 0x05;

        public readonly Entity Entity;

        public SpawnPlayer(Entity entity)
        {
            Entity = entity;
        }

        public void Write(IPacketWriter writer)
        {
            writer.WriteVarInt(Entity.Id);
            writer.WriteGuid(Entity.Guid);
            writer.WriteDouble(Entity.Position.X);
            writer.WriteDouble(Entity.Position.Y);
            writer.WriteDouble(Entity.Position.Z);
            
            var rotation = RotationHelper.FromLookAt(Entity.LookDir);
            var pitch = rotation.X;
            var yaw = rotation.Y;
            writer.WriteUInt8(RotationHelper.RadiansTo256Angle(pitch));
            writer.WriteUInt8(RotationHelper.RadiansTo256Angle(yaw));
        }
    }
}
