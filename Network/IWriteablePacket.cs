namespace SM3.Network
{
    public interface IWriteablePacket
    {
        int Id { get; }

        void Write(IPacketWriter writer);
    }
}