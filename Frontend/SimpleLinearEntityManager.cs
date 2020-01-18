using App.Metrics;

namespace Frontend
{
    public sealed class SimpleLinearEntityManager : IEntityManager
    {
        private readonly IMetrics _metrics;
        private int _nextId = 0;
     
        public SimpleLinearEntityManager(IMetrics metrics)
        {
            _metrics = metrics;
        }

        public IEntityId ReserveEntityId()
        {
            _metrics.Measure.Meter.Mark(MetricsRegistry.EntityIdReserved);
            return new SimpleEntityId(_nextId++);
        }

        private readonly struct SimpleEntityId : IEntityId
        {
            public int Value { get; }

            public SimpleEntityId(int value)
            {
                Value = value;
            }

            public void Dispose()
            { }
        }
    }
}