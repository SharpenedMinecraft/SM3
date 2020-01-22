using System.Collections.Generic;

namespace Frontend
{
    public interface IWindow
    {
        byte Id { get; set; }
        IEnumerable<IWriteablePacket> OpenPackets { get; }
        IEnumerable<IWriteablePacket> ClosePackets { get; }
    }
}