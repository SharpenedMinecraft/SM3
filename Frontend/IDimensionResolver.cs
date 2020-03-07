using System.Collections.Generic;

namespace SM3.Frontend
{
    public interface IDimensionResolver
    {
        IDimension GetDimension(int id);
        IEnumerable<IDimension> GetDimensions();
    }
}