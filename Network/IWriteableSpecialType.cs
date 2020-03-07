namespace SM3.Network
{
    public interface IWriteableSpecialType
    {
        void Write(IPacketWriter writer);
    }
}
