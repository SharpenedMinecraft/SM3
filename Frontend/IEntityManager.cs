using System;
using System.Collections.Generic;

namespace SM3.Frontend
{
    public interface IEntityManager
    {
        Entity Create<T>() where T : unmanaged, IEntity;
        void Destroy<T>(Entity entity);
    }
}
