namespace Frontend
{
    public interface IEntityRegistry
    {
        int this[string id] { get; }
    }
}