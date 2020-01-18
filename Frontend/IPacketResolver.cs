namespace Frontend
{
    public interface IPacketResolver
    {
        IReadablePacket? GetReadablePacket(int id, IConnectionState connectionState);
    }
}