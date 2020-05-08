using System;
using Messaging.Messages;

namespace Messaging
{
    public interface IMessagingProvider
    {
        IObservable<ClientHandshake> WhenClientHandshakes { get; }
        void OnClientHandshake(ClientHandshake clientHandshake);
    }
}