using System.Collections.Generic;

namespace Frontend
{
    public interface ITagProvider
    {
        IReadOnlyDictionary<string, Tag> FluidTags { get; }
        IReadOnlyDictionary<string, Tag> BlockTags { get; }
        IReadOnlyDictionary<string, Tag> EntityTypes { get; }
        IReadOnlyDictionary<string, Tag> ItemTags { get; }
    }
}