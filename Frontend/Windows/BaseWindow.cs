using System.Collections.Generic;
using Frontend.Packets.Play;

namespace Frontend.Windows
{
    public abstract class BaseWindow : IWindow
    {
        public byte Id { get; set; }

        public IEnumerable<IWriteablePacket> OpenPackets
        {
            get { yield return new OpenWindow(this); }
        }

        public IEnumerable<IWriteablePacket> ClosePackets
        {
            get { yield return new ClientboundCloseWindow(this); }
        }

        private static Chat _defaultTitle = new ChatBuilder()
            .AppendText("DEFAULT TITLE")
            .WithColor("red")
            .Build();

        public virtual Chat Title => _defaultTitle;
        public abstract string Type { get; }
        public int TypeId { get; }
        public abstract int SlotCount { get; }
        public int InventoryToWindowIndex(int index) => SlotCount + index;

        public BaseWindow(IWindowRegistry windowRegistry)
        {
            TypeId = windowRegistry[Type];
        }
    }
}