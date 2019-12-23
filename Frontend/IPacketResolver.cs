namespace Frontend
{
    public interface IPacketResolver
    {
        IPacket? GetPacket(int id, IConnectionState connectionState, bool serverBound);
    }
}