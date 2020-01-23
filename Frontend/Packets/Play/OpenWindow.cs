namespace Frontend.Packets.Play
{
    public readonly struct OpenWindow : IWriteablePacket
    {
        public int Id => 0x2F;

        public readonly IMenu Window;

        public OpenWindow(IMenu window)
        {
            Window = window;
        }

        public void Write(IPacketWriter writer)
        {
            writer.WriteVarInt(Window.Id);
            writer.WriteVarInt(Window.TypeId);
            writer.WriteSpecialType(Window.Title);
        }
    }
}