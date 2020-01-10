namespace Frontend
{
    public readonly struct BlockPosition
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Z;

        public BlockPosition(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static implicit operator ChunkPosition(BlockPosition position)
            => position.ToChunkPosition();

        public ChunkPosition ToChunkPosition()
        {
            var cx = X >> 4;
            var cz = Z >> 4;
            
            return new ChunkPosition(cx, cz);
        }

        public void Deconstruct(out int x, out int y, out int z)
        {
            x = X;
            y = Y;
            z = Z;
        }
        
        public static implicit operator BlockPosition((int x, int y, int z) tuple) 
            => new BlockPosition(tuple.x, tuple.y, tuple.z);
    }
}