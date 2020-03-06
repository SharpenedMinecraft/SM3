using System.Collections.Generic;
using System.Linq;
using App.Metrics;

namespace SM3.Frontend
{
    public sealed class MCDimensionResolver : IDimensionResolver
    {
        private readonly IMetrics _metrics;
        private Dictionary<int, IDimension> _dimensions;

        public MCDimensionResolver(IEnumerable<IDimension> dimensions, IMetrics metrics)
        {
            _metrics = metrics;
            _dimensions = dimensions.ToDictionary(x => x.Id, x => x);
        }

        public IDimension GetDimension(int id)
        {
            _metrics.Measure.Meter.Mark(MetricsRegistry.DimensionResolved);
            return _dimensions[id];
        }

        public IEnumerable<IDimension> GetDimensions()
        {
            return _dimensions.Values;
        }
    }
}