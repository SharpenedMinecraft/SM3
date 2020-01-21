using System;

namespace Frontend.Entities
{
    public abstract class Throwable : ObjectEntity
    {
        protected Throwable(IEntityRegistry entityRegistry) : base(entityRegistry)
        { }
    }
}