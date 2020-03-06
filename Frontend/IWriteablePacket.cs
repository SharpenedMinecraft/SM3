namespace SM3.Frontend
{
    public interface IWriteablePacket
    {
        int Id { get; }

        void Write(IPacketWriter writer);
    }
}