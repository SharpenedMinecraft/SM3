using System;
using System.Buffers;
using App.Metrics;

namespace Frontend
{
    public sealed class MCPacketWriterFactory : IPacketWriterFactory
    {
        private readonly IMetrics _metrics;

        public MCPacketWriterFactory(IMetrics metrics)
        {
            _metrics = metrics;
        }
        
        public IPacketWriter CreateWriter(Memory<byte> memory)
        {
            _metrics.Measure.Meter.Mark(MetricsRegistry.PacketWriters);
            return new MCPacketWriter(memory);
        }
    }
}