using System;
using System.Diagnostics;

namespace Frontend
{
    public readonly struct ReadOnlyChunk
    {
        public readonly ReadOnlyMemory<BlockState> States;
        /// <summary>
        /// Every byte contains two values
        /// </summary>
        public readonly ReadOnlyMemory<byte> Skylight;
        /// <summary>
        /// Every byte contains two values
        /// </summary>
        public readonly ReadOnlyMemory<byte> Blocklight;
        
        public const int Width = 16;
        public const int Height = 256;
        public const int Depth = 16;

        public ReadOnlyChunk(ReadOnlyMemory<BlockState> states, ReadOnlyMemory<byte> skylight, ReadOnlyMemory<byte> blocklight)
        {
            States = states;
            Skylight = skylight;
            Blocklight = blocklight;
            Debug.Assert(States.Length == Width * Height * Depth);
            Debug.Assert(Skylight.Length == (Width * Height * Depth) / 2);
            Debug.Assert(Blocklight.Length == (Width * Height * Depth) / 2);
        }

        public int CalculateIndex(int x, int y, int z)
            => x + Width * (y + Height * z);

        private BlockState this[int x, int y, int z] => States.Span[CalculateIndex(x, y, z)];
    }
}