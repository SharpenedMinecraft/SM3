namespace Frontend
{
    public readonly struct ChunkPosition
    {
        public readonly int X;
        public readonly int Z;

        public ChunkPosition(int x, int z)
        {
            X = x;
            Z = z;
        }

        public void Deconstruct(out int x, out int z)
        {
            x = X;
            z = Z;
        }

        public static implicit operator ChunkPosition((int x, int z) tuple)
            => From(tuple);
        
        public static ChunkPosition From((int x, int z) tuple) 
            => new ChunkPosition(tuple.x, tuple.z);
    }
}