namespace Frontend
{
    public interface IPacketQueue
    {
        bool NeedsWriting { get; }
        
        void WriteQueued();
        void Write(IPacket packet);
        void WriteImmediate(IPacket packet);
    }
}