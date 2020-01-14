using System;

namespace Frontend
{
    public interface IConnectionState
    {
        MCConnectionStage ConnectionStage { get; set; }

        IPacketQueue PacketQueue { get; }

        bool IsLocal { get; set; }
        
        Player PlayerEntity { get; set; }
    }
}