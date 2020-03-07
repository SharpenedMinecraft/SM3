namespace SM3.Network
{
    public interface IPacketQueue
    {
        bool NeedsWriting { get; }
        
        void WriteQueued();
        void Write(IWriteablePacket packet);
        void WriteImmediate(IWriteablePacket packet);
    }
}