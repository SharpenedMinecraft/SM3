using System.Collections.Generic;
using System.Linq;

namespace Frontend
{
    public sealed class MCDimensionResolver : IDimensionResolver
    {
        private Dictionary<int, IDimension> _dimensions;

        public MCDimensionResolver(IEnumerable<IDimension> dimensions)
        {
            _dimensions = dimensions.ToDictionary(x => x.Id, x => x);
        }

        public IDimension GetDimension(int id) => _dimensions[id];

        public IEnumerable<IDimension> GetDimensions() => _dimensions.Values;
    }
}