using System.Buffers;
using App.Metrics;

namespace Frontend
{
    public sealed class MCPacketReaderFactory : IPacketReaderFactory
    {
        private readonly IMetrics _metrics;

        public MCPacketReaderFactory(IMetrics metrics)
        {
            _metrics = metrics;
        }

        public IPacketReader CreateReader(ReadOnlySequence<byte> buffer)
        {
            _metrics.Measure.Meter.Mark(MetricsRegistry.PacketReaders);
            return new MCPacketReader(buffer);
        }
    }
}