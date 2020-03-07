namespace SM3.Entities
{
    public sealed class Egg : ItemedThrowable
    {
        public override string Type => "minecraft:egg";

        public Egg(Entity entity, IEntityRegistry entityRegistry) : base(entity, entityRegistry)
        { }
    }
}
