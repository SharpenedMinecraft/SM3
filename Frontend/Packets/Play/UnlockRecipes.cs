namespace SM3.Frontend.Packets.Play
{
    public readonly struct UnlockRecipes : IWriteablePacket
    {
        public int Id => 0x37;
        
        public void Write(IPacketWriter writer)
        {
            // this will be revisited once I implement recipes, but for now, all is disabled
            writer.WriteVarInt(0);
            writer.WriteBoolean(false);
            writer.WriteBoolean(false);
            writer.WriteBoolean(false);
            writer.WriteBoolean(false);
            writer.WriteVarInt(0);
            writer.WriteVarInt(0);
        }
    }
}