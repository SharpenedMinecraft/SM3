using System;
using Messaging;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Frontend
{
    public interface IReadablePacket
    {
        void Read(IPacketReader reader);

        bool Validate(ILogger logger);

        void UpdateState(IConnectionState state);
        
        void Message(IMessagingProvider messagingProvider);
    }
}