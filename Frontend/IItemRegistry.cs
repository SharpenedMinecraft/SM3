namespace Frontend
{
    public interface IItemRegistry
    {
        string Default { get; }
        int this[string id] { get; }
    }
}