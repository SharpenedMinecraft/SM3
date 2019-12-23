using System.IO.Pipes;
using Microsoft.Extensions.Logging;

namespace Frontend
{
    public interface IPacket
    {
        int Id { get; }
        bool IsServerbound { get; }
        MCConnectionStage Stage { get; }

        int Size { get; }
        
        void Write(IPacketWriter writer);
        void Read(IPacketReader reader);

        void Process(ILogger logger, IConnectionState connectionState, IPacketQueue packetQueue);
    }
}