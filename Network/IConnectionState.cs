namespace SM3.Network
{
    public interface IConnectionState
    {
        ConnectionStage ConnectionStage { get; set; }

        IPacketQueue PacketQueue { get; }

        bool IsLocal { get; set; }
        
        Entity? PlayerEntity { get; set; }
    }
}