namespace Frontend
{
    public interface IMenuRegistry
    {
        int this[string id] { get; }
    }
}