using System;
using System.Numerics;

namespace Frontend.Packets.Play
{
    public readonly struct PlayerPositionAndLook : IWriteablePacket
    {
        [Flags]
        public enum Flags : byte
        {
            None = 0,
            RelativeX = 0x01,
            RelativeY = 0x02,
            RelativeZ = 0x04,
            RelativeYRot = 0x08,
            RelativeXRot = 0x10,
        }

        public int Id => 0x35;

        public readonly Vector3 Position;
        public readonly Vector2 Rotation;
        public readonly Flags RelativeFlags;
        public readonly int TeleportId;

        public PlayerPositionAndLook(Vector3 position, Vector2 rotation, Flags relativeFlags, int teleportId)
        {
            Position = position;
            Rotation = rotation;
            RelativeFlags = relativeFlags;
            TeleportId = teleportId;
        }

        public void Write(IPacketWriter writer)
        {
            writer.WriteDouble(Position.X);
            writer.WriteDouble(Position.Y);
            writer.WriteDouble(Position.Z);
            writer.WriteSingle(Rotation.X);
            writer.WriteSingle(Rotation.Y);
            writer.WriteUInt8((byte)RelativeFlags);
            writer.WriteVarInt(TeleportId);
        }
    }
}