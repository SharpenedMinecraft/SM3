using System.Collections.Generic;

namespace Frontend
{
    public interface IDimensionResolver
    {
        IDimension GetDimension(int id);
        IEnumerable<IDimension> GetDimensions();
    }
}