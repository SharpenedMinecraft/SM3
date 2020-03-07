using System;
using System.IO.Pipelines;
using SM3.Network;

namespace SM3.Frontend
{
    public sealed class MCPacketQueueFactory : IPacketQueueFactory
    {
        private readonly IPacketWriterFactory _writerFactory;
        private readonly IBroadcastQueue _broadcastQueue;
        private readonly IServiceProvider _serviceProvider;

        public MCPacketQueueFactory(IPacketWriterFactory writerFactory, IBroadcastQueue broadcastQueue, IServiceProvider serviceProvider)
        {
            _writerFactory = writerFactory;
            _broadcastQueue = broadcastQueue;
            _serviceProvider = serviceProvider;
        }

        public IPacketQueue CreateQueue(PipeWriter writer)
        {
            return new MCPacketQueue(writer, _writerFactory, _broadcastQueue, _serviceProvider);
        }
    }
}