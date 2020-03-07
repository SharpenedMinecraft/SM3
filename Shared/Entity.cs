using System;

namespace SM3
{
    public readonly struct Entity
    {
        public readonly Guid Guid;
        public readonly int NumericId;

        public Entity(Guid guid, int numericId)
        {
            Guid = guid;
            NumericId = numericId;
        }
    }
}