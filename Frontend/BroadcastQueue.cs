using System.Collections.Generic;
using App.Metrics;

namespace Frontend
{
    public sealed class BroadcastQueue : IBroadcastQueue
    {
        private List<IPacketQueue> _packetQueue = new List<IPacketQueue>();
        private IMetrics _metrics;

        public BroadcastQueue(IMetrics metrics)
        {
            _metrics = metrics;
        }

        public void Broadcast(IWriteablePacket packet)
        {
            _metrics.Measure.Meter.Mark(MetricsRegistry.BroadcastPackets);
            foreach (var queue in _packetQueue)
            {
                // TODO: it might be worth caching some of this.
                // it's not perfectly valid though
                // I could make a separate IConnectionState
                // and fallback if any property is accessed.
                // but I'm not sure the performance impact...
                queue.Write(packet);
            }
        }

        public void Register(IPacketQueue queue)
        {
            _packetQueue.Add(queue);
        }

        public void Deregister(IPacketQueue queue)
        {
            _packetQueue.Remove(queue);
        }
    }
}