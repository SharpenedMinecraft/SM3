namespace Frontend
{
    public interface IBroadcastQueue
    {
        void Broadcast(IWriteablePacket packet);
        void Register(IPacketQueue queue);
    }
}