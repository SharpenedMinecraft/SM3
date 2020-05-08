using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Messaging.Messages;

namespace Messaging
{
    internal class LocalMessagingProvider : IMessagingProvider
    {
        private readonly ISubject<ClientHandshake> _whenClientHandshales = new Subject<ClientHandshake>();
        public IObservable<ClientHandshake> WhenClientHandshakes => _whenClientHandshales;

        public void OnClientHandshake(ClientHandshake clientHandshake)
        {
            _whenClientHandshales.OnNext(clientHandshake);
        }
    }
}