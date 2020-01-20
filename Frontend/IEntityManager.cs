using System;
using System.Collections.Generic;

namespace Frontend
{
    public interface IEntityManager : IDisposable, IEnumerable<IEntity>

    {
    // General Use
    ref T Instantiate<T>() where T : IEntity;
    void Destroy<T>(T entity) where T : IEntity;
    IEnumerable<T> EnumerateEntities<T>() where T : IEntity;
    int Count { get; }

    ref T GetEntity<T>(int id) where T : IEntity;

    // Advanced Use, take care
    // Called to copy Pre-Tick and create Post-Tick
    IEntityManager Copy();
    }
}
