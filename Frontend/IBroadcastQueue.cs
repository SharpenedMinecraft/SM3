namespace SM3.Frontend
{
    public interface IBroadcastQueue
    {
        void Broadcast(IWriteablePacket packet);
        void Register(IPacketQueue queue);
    }
}