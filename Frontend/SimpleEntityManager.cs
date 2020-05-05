using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Frontend.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Frontend
{
    public sealed class SimpleEntityManager : IEntityManager
    {
        private const int MinSize = 8;
        private const int MaxUpsize = 2048;
        private IEntity?[] _entities;
        private SemaphoreSlim _entitiesSemaphore;
        private int _index; // index 0 is skipped. Id 0 is invalid.
        private readonly IServiceProvider _provider;
        private readonly IBroadcastQueue _queue;

        public SimpleEntityManager(IServiceProvider provider) : this(provider, 0, new IEntity[MinSize])
        { }


        private SimpleEntityManager(IServiceProvider provider, int index, IEntity[] entities)
        {
            _index = index;
            _entities = entities;
            _entitiesSemaphore = new SemaphoreSlim(1, 1);
            _provider = provider;
            _queue = provider.GetRequiredService<IBroadcastQueue>();
        }


        public ref T Instantiate<T>() where T : IEntity
        {
            var id = Interlocked.Increment(ref _index);
            if (id >= _entities.Length)
            {
                _entitiesSemaphore.Wait();
                try
                {
                    if (id >= _entities.Length)
                    {
                        Array.Resize(ref _entities, _entities.Length + Math.Min(_entities.Length, MaxUpsize));
                    }
                }
                finally
                {
                    _entitiesSemaphore.Release();
                }
            }

            var v = ActivatorUtilities.CreateInstance<T>(_provider);
            v.Id = id;
            _entities[id] = v;
            return ref GetEntity<T>(id)!;
        }

        public void Destroy<T>(T entity) where T : IEntity
        {
            _entities[entity.Id] = null;
        }

        public IEnumerable<T> EnumerateEntities<T>() where T : IEntity
        {
            foreach (var entity in _entities)
            {
                if (!(entity is null) && entity is T t)
                    yield return t;
            }
        }

        public int Count => _entities.Length;

        [return: MaybeNull]
        public ref T GetEntity<T>(int id) where T : IEntity
        {
            if (id == 0)
                throw new ArgumentException("Id may not be 0", nameof(id));

            return ref Unsafe.As<IEntity, T>(ref _entities[id]!)!;
        }

        public IEntityManager Copy()
        {
            _entitiesSemaphore.Wait();
            try
            {
                var newEntities = new IEntity[_entities.Length];
                Array.Copy(_entities, newEntities, _entities.Length);
                return new SimpleEntityManager(_provider, _index, newEntities);
            }
            finally
            {
                _entitiesSemaphore.Release();
            }
        }

        public void Spawn(IEntity entity)
        {
            foreach (var packet in entity.SpawnPackets)
            {
                _queue.Broadcast(packet);
            }
        }

        public void Dispose()
        {
            _entitiesSemaphore?.Dispose();
        }

        public IEnumerator<IEntity> GetEnumerator()
        {
            foreach (var entity in _entities)
            {
                if (!(entity is null))
                    yield return entity;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
