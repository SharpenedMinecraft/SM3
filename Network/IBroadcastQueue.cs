namespace SM3.Network
{
    public interface IBroadcastQueue
    {
        void Broadcast(IWriteablePacket packet);
        void Register(IPacketQueue queue);
    }
}