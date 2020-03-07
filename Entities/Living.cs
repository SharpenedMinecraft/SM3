using SM3.Network;

namespace SM3.Entities
{
    public abstract class Living : ObjectEntity
    {
        public Hand? ActiveHand { get; set; }
        public bool InRiptideSpinAttack { get; set; }
        public float Health { get; set; }
        public int PotionEffectColor { get; set; }
        /// <summary>
        /// Reduces number of particles to 1/15
        /// </summary>
        public bool IsPotionEffectAmbient { get; set; }
        public int ArrowsInEntity { get; set; }
        public int AbsorptionAmount { get; set; }
        public Vector3Int? SleepingPosition { get; set; }


        protected Living(Entity entity, IEntityRegistry entityRegistry) : base(entity, entityRegistry)
        { }

        public override void WriteMetadata(EntityMetadata metadata)
        {
            base.WriteMetadata(metadata);
            var bits = (byte)0;
            if (!(ActiveHand is null))
            {
                bits |= 0x01;
                bits |= (byte)(ActiveHand.Value == Hand.Main ? 0 : 1);
            }

            if (InRiptideSpinAttack)
                bits |= 0x04;

            metadata.WriteUInt8(7, bits );
            metadata.WriteSingle(8, Health);
            metadata.WriteVarInt(9, PotionEffectColor);
            metadata.WriteBoolean(10, IsPotionEffectAmbient);
            metadata.WriteVarInt(11, ArrowsInEntity);
            metadata.WriteVarInt(12, AbsorptionAmount);
            metadata.WriteOptPosition(13, SleepingPosition);
        }
    }

    public enum Hand
    {
        Main = 0,
        Off = 1
    }
}
