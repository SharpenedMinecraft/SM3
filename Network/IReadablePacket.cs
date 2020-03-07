using System;
using Microsoft.Extensions.Logging;

namespace SM3.Network
{
    public interface IReadablePacket
    {
        int Id { get; }
        ConnectionStage Stage { get; }
        
        void Read(IPacketReader reader);

        void Process(ILogger logger, IConnectionState state, IServiceProvider serviceProvider);
    }
}