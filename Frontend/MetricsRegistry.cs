using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Histogram;
using App.Metrics.Meter;

namespace SM3.Frontend
{
    public static class MetricsRegistry
    {
        public static readonly CounterOptions ActiveConnections = new CounterOptions
        {
            MeasurementUnit = Unit.Connections,
            Name = "Active Connections",
        };

        public static readonly MeterOptions StatusRequests = new MeterOptions
        {
            MeasurementUnit = Unit.Requests,
            Name = "Status Requests"
        };

        public static readonly MeterOptions LoginRequests = new MeterOptions
        {
            MeasurementUnit = Unit.Requests,
            Name = "Login Requests"
        };

        public static readonly HistogramOptions ReadPacketSize = new HistogramOptions
        {
            MeasurementUnit = Unit.Bytes,
            Name = "Read Packet Size"
        };
        
        public static readonly HistogramOptions WritePacketSize = new HistogramOptions
        {
            MeasurementUnit = Unit.Bytes,
            Name = "Write Packet Size"
        };

        public static readonly MeterOptions PacketReaders = new MeterOptions
        {
            MeasurementUnit = Unit.Calls,
            Name = "Packet Readers"
        };
        
        public static readonly MeterOptions PacketWriters = new MeterOptions
        {
            MeasurementUnit = Unit.Calls,
            Name = "Packet Writers"
        };

        public static readonly MeterOptions PacketQueues = new MeterOptions
        {
            MeasurementUnit = Unit.Calls,
            Name = "Packet Queues"
        };

        public static readonly MeterOptions EntityIdReserved = new MeterOptions
        {
            MeasurementUnit = Unit.None,
            Name = "Entity Id Reserved"
        };

        public static readonly MeterOptions DimensionResolved = new MeterOptions
        {
            MeasurementUnit = Unit.Calls,
            Name = "Dimension Resolved"
        };

        public static readonly MeterOptions BroadcastPackets = new MeterOptions
        {
            MeasurementUnit = Unit.Calls,
            Name = "Broadcast Packets"
        };
    }
}