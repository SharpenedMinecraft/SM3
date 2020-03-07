namespace SM3
{
    public interface IEntityRegistry
    {
        string Default { get; }
        int this[string id] { get; }
    }
}
