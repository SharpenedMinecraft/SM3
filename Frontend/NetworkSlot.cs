namespace Frontend
{
    public readonly struct NetworkSlot : IWriteableSpecialType
    {
        public readonly bool Present;
        public readonly int Id;
        public readonly sbyte Count;
        public readonly NbtCompound? Nbt;

        public NetworkSlot(bool present, int id, sbyte count, NbtCompound? nbt)
        {
            Present = present;
            Id = id;
            Count = count;
            Nbt = nbt;
        }

        public void Write(IPacketWriter writer)
        {
            writer.WriteBoolean(Present);

            if (Present)
            {
                writer.WriteVarInt(Id);
                writer.WriteInt8(Count);
                writer.WriteNbt(Nbt);
            }
        }
    }
}
