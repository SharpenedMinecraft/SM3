using System;
using System.Collections.Generic;
using App.Metrics;

namespace SM3.Frontend
{
    public sealed class BroadcastQueue : IBroadcastQueue
    {
        private List<WeakReference<IPacketQueue>> _packetQueues = new List<WeakReference<IPacketQueue>>();
        private IMetrics _metrics;

        public BroadcastQueue(IMetrics metrics)
        {
            _metrics = metrics;
        }

        public void Broadcast(IWriteablePacket packet)
        {
            _metrics.Measure.Meter.Mark(MetricsRegistry.BroadcastPackets);
            for (var index = 0; index < _packetQueues.Count; index++)
            {
                var reference = _packetQueues[index];
                // TODO: it might be worth caching some of this.
                // it's not perfectly valid though
                // I could make a separate IConnectionState
                // and fallback if any property is accessed.
                // but I'm not sure the performance impact...
                if (reference.TryGetTarget(out var queue))
                {
                    queue.Write(packet);
                }
                else
                {
                    _packetQueues.Remove(reference);
                    index--;
                }
            }
        }

        public void Register(IPacketQueue queue)
        {
            _packetQueues.Add(new WeakReference<IPacketQueue>(queue));
        }
    }
}