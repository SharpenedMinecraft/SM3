namespace Frontend
{
    public interface IPacketQueue
    {
        bool NeedsWriting { get; }
        
        void WriteQueued();
        void Write(IWriteablePacket packet);
        void WriteImmediate(IWriteablePacket packet);
    }
}