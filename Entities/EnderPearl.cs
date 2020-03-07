namespace SM3.Entities
{
    public sealed class EnderPearl: ItemedThrowable
    {
        public override string Type => "minecraft:ender_pearl";

        public EnderPearl(Entity entity, IEntityRegistry entityRegistry) : base(entity, entityRegistry)
        { }
    }
}
