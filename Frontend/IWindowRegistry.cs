namespace Frontend
{
    public interface IWindowRegistry
    {
        int this[string id] { get; }
    }
}