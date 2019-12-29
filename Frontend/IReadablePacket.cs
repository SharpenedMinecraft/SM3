using System;
using Microsoft.Extensions.Logging;

namespace Frontend
{
    public interface IReadablePacket
    {
        int Id { get; }
        MCConnectionStage Stage { get; }
        
        void Read(IPacketReader reader);

        void Process(ILogger logger, IConnectionState state, IServiceProvider serviceProvider);
    }
}