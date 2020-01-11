using System;
using System.Diagnostics;

namespace Frontend
{
    public readonly struct ReadOnlyChunk
    {
        public readonly ReadOnlyMemory<BlockState> States;
        public const int Width = 16;
        public const int Height = 256;
        public const int Depth = 16;

        public ReadOnlyChunk(ReadOnlyMemory<BlockState> states)
        {
            States = states;
            Debug.Assert(States.Length == Width * Height * Depth);
        }

        public int CalculateIndex(int x, int y, int z)
            => x + Width * (y + Height * z);

        private BlockState this[int x, int y, int z] => States.Span[CalculateIndex(x, y, z)];
    }
}