namespace Frontend.Packets.Play
{
    public readonly struct ClientboundCloseWindow : IWriteablePacket
    {
        public int Id => 0x14;

        public readonly IWindow Window;

        public ClientboundCloseWindow(IWindow window)
        {
            Window = window;
        }

        public void Write(IPacketWriter writer)
        {
            writer.WriteInt8(Window.Id);
        }
    }
}