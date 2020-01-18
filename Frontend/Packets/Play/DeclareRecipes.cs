namespace Frontend.Packets.Play
{
    // TODO: Implement Recipes as a concept
    // then implement this packet like https://wiki.vg/Protocol#Declare_Recipes
    public readonly struct DeclareRecipes : IWriteablePacket
    {
        public int Id => 0x5B;
        
        // public Recipe[] Recipes;

        public void Write(IPacketWriter writer)
        {
            writer.WriteVarInt(0);
        }
    }
}