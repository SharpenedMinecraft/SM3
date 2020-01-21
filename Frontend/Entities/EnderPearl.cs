using System;

namespace Frontend.Entities
{
    public sealed class EnderPearl: ItemedThrowable
    {
        public override string Type => "minecraft:ender_pearl";

        public EnderPearl(IEntityRegistry entityRegistry) : base(entityRegistry)
        { }
    }
}