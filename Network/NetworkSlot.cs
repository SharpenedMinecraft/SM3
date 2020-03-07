using SM3.NBT;

namespace SM3.Network
{
    public readonly struct NetworkSlot : IWriteableSpecialType
    {
        public readonly bool Present;
        public readonly int Id;
        public readonly byte Count;
        public readonly NbtCompound? Nbt;

        public void Write(IPacketWriter writer)
        {
            writer.WriteBoolean(Present);

            if (Present)
            {
                writer.WriteVarInt(Id);
                writer.WriteUInt8(Count);
                writer.WriteNbt(Nbt);
            }
        }
    }
}
