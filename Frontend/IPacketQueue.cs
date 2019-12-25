namespace Frontend
{
    public interface IPacketQueue
    {
        bool NeedsWriting { get; }
        
        void WriteQueued();
        void Write<T>(params object[] parameters) where T : IPacket;
        void WriteImmediate<T>(params object[] parameters) where T : IPacket;
    }
}