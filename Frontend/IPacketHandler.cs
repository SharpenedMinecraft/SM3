namespace Frontend
{
    public interface IPacketHandler
    {
        void HandlePacket(MCConnectionContext ctx, IPacketReader reader, IPacketQueue packetQueue, int id);
    }
}