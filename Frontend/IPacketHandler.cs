namespace Frontend
{
    public interface IPacketHandler
    {
        void HandlePacket(MCConnectionContext ctx, IPacketReader reader, IPacketWriterFactory writerFactory, int id);
    }
}