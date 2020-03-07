using SM3.Network;

namespace SM3.Frontend
{
    public interface IPacketResolver
    {
        IReadablePacket? GetReadablePacket(int id, IConnectionState connectionState);
    }
}