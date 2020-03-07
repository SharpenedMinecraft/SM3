using SM3.Network;

namespace SM3.Frontend
{
    public interface IPacketHandler
    {
        void HandlePacket(MCConnectionContext ctx, IPacketReader reader, IPacketQueue packetQueue, int id);
    }
}