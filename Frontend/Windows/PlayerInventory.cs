using System;
using System.Collections.Generic;
using System.Linq;
using Frontend.Packets.Play;

namespace Frontend.Windows
{
    public sealed class PlayerInventory : IWindow
    {
        public byte Id
        {
            get => 0;
            set => throw new InvalidOperationException("Player Inventory's ID is always 0");
        }
        
        public IEnumerable<IWriteablePacket> OpenPackets => throw new InvalidOperationException("Player Inventory cannot be opened explicitly");

        public IEnumerable<IWriteablePacket> ClosePackets => Enumerable.Empty<IWriteablePacket>();

        public Chat Title => throw new InvalidOperationException("Player Inventory does not have a Title");
        public string Type => throw new InvalidOperationException("Player Inventory does not have a type");
        public int TypeId => throw new InvalidOperationException("Player Inventory does not have a type Id");
        public int InventoryToWindowIndex(int index) => index;
    }
}