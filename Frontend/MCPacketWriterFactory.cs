using System;
using App.Metrics;

namespace SM3.Frontend
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