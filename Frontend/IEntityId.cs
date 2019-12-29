using System;

namespace Frontend
{
    public interface IEntityId : IDisposable
    {
        int Id { get; }
    }
}