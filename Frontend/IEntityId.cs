using System;

namespace Frontend
{
    public interface IEntityId : IDisposable
    {
        int Value { get; }
    }
}