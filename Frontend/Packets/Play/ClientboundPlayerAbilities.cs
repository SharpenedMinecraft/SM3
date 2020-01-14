using System;

namespace Frontend.Packets.Play
{
    public readonly struct ClientboundPlayerAbilities : IWriteablePacket
    {
        [Flags]
        public enum Flags : byte
        {
            None = 0x00,
            Invulnerable = 0x01,
            Flying = 0x02,
            AllowFlying = 0x04,
            InstantBreak = 0x08
        }
        
        public int Id => 0x31;

        public readonly Flags Abilities;
        public readonly float FlyingSpeed;
        public readonly float FoVModifier;

        public ClientboundPlayerAbilities(Flags abilities, float flyingSpeed, float foVModifier)
        {
            Abilities = abilities;
            FlyingSpeed = flyingSpeed;
            FoVModifier = foVModifier;
        }

        public void Write(IPacketWriter writer)
        {
            writer.WriteUInt8((byte)Abilities);
            writer.WriteSingle(FlyingSpeed);
            writer.WriteSingle(FoVModifier);
        }
    }
}