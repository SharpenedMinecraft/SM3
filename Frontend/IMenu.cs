using System.Collections.Generic;

namespace Frontend
{
    public interface IMenu
    {
        byte Id { get; set; }
        IEnumerable<IWriteablePacket> OpenPackets { get; }
        IEnumerable<IWriteablePacket> ClosePackets { get; }
        Chat Title { get; }
        string Type { get; }
        int TypeId { get; }
    }
}