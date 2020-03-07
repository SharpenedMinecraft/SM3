using System;

namespace SM3
{
    public readonly struct Entity
    {
        public readonly Guid Uuid;
        public readonly int NumericId;

        public Entity(Guid uuid, int numericId)
        {
            Uuid = uuid;
            NumericId = numericId;
        }
    }
}