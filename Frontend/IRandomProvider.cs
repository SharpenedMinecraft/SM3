namespace Frontend
{
    public interface IRandomProvider
    {
        long Seed { get; }
        int Next(int bits);
        int NextInt() => Next(32);
    }
}