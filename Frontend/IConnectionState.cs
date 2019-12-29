using System;

namespace Frontend
{
    public interface IConnectionState
    {
        MCConnectionStage ConnectionStage { get; set; }

        IPacketQueue PacketQueue { get; }

        bool IsLocal { get; set; }

        Guid Guid { get; set; }
        IEntityId PlayerEntityId { get; set; }
    }
}