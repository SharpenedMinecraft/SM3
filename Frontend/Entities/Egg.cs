namespace SM3.Frontend.Entities
{
    public sealed class Egg : ItemedThrowable
    {
        public override string Type => "minecraft:egg";

        public Egg(IEntityRegistry entityRegistry) : base(entityRegistry)
        { }
    }
}
