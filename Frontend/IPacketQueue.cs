using System;

namespace Frontend
{
    public interface IPacketQueue : IDisposable
    {
        bool NeedsWriting { get; }
        
        void WriteQueued();
        void Write(IWriteablePacket packet);
        void WriteImmediate(IWriteablePacket packet);
    }
}