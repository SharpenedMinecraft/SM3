using SM3.Frontend.Entities;

namespace SM3.Frontend
{
    public interface IConnectionState
    {
        MCConnectionStage ConnectionStage { get; set; }

        IPacketQueue PacketQueue { get; }

        bool IsLocal { get; set; }
        
        Player PlayerEntity { get; set; }
    }
}