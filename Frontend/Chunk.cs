using System;
using System.Diagnostics;

namespace Frontend
{
    public struct Chunk
    {
        public Memory<BlockState> States;
        public const int Width = 16;
        public const int Height = 256;
        public const int Depth = 16;

        public Chunk(Memory<BlockState> states)
        {
            States = states;
            Debug.Assert(States.Length == Width * Height * Depth);
        }

        public readonly int CalculateIndex(int x, int y, int z)
            => x + Width * (y + Height * z);

        private ref BlockState this[int x, int y, int z] => ref States.Span[CalculateIndex(x, y, z)];

        public static implicit operator ReadOnlyChunk(Chunk c)
            => c.ToReadOnlyChunk();

        public ReadOnlyChunk ToReadOnlyChunk()
            => new ReadOnlyChunk(States);
    }
}