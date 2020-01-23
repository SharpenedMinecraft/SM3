namespace Frontend.Packets.Play
{
    public readonly struct ClientboundCloseWindow : IWriteablePacket
    {
        public int Id => 0x14;

        public readonly IMenu Window;

        public ClientboundCloseWindow(IMenu window)
        {
            Window = window;
        }

        public void Write(IPacketWriter writer)
        {
            writer.WriteUInt8(Window.Id);
        }
    }
}