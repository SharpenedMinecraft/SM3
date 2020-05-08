using System;

namespace Frontend
{
    public interface IConnectionState
    {
        MCConnectionStage ConnectionStage { get; set; }

        IPacketQueue PacketQueue { get; }

        Player PlayerEntity { get; set; }
    }
}