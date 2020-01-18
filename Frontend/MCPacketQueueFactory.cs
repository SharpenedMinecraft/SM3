using System;
using System.IO.Pipelines;
using App.Metrics;

namespace Frontend
{
    public sealed class MCPacketQueueFactory : IPacketQueueFactory
    {
        private readonly IPacketWriterFactory _writerFactory;
        private readonly IMetrics _metrics;
        private readonly IBroadcastQueue _broadcastQueue;
        private readonly IServiceProvider _serviceProvider;

        public MCPacketQueueFactory(IPacketWriterFactory writerFactory, IMetrics metrics, IBroadcastQueue broadcastQueue, IServiceProvider serviceProvider)
        {
            _writerFactory = writerFactory;
            _metrics = metrics;
            _broadcastQueue = broadcastQueue;
            _serviceProvider = serviceProvider;
        }

        public IPacketQueue CreateQueue(PipeWriter writer)
        {
            _metrics.Measure.Meter.Mark(MetricsRegistry.PacketQueues);
            return new MCPacketQueue(writer, _writerFactory, _metrics, _broadcastQueue, _serviceProvider);
        }
    }
}