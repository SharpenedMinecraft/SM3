using System;
using System.Diagnostics;

namespace Frontend
{
    public struct Chunk
    {
        public Memory<BlockState> States;
        /// <summary>
        /// Every byte contains two values
        /// </summary>
        public Memory<byte> Skylight;
        /// <summary>
        /// Every byte contains two values
        /// </summary>
        public Memory<byte> Blocklight;
        
        public const int Width = 16;
        public const int Height = 256;
        public const int Depth = 16;

        public Chunk(Memory<BlockState> states, Memory<byte> skylight, Memory<byte> blocklight)
        {
            States = states;
            Skylight = skylight;
            Blocklight = blocklight;
            Debug.Assert(States.Length == Width * Height * Depth);
            Debug.Assert(Skylight.Length == (Width * Height * Depth) / 2);
            Debug.Assert(Blocklight.Length == (Width * Height * Depth) / 2);
        }

        public readonly int CalculateIndex(int x, int y, int z)
            => x + Width * (z + Depth * y);

        private ref BlockState this[int x, int y, int z] => ref States.Span[CalculateIndex(x, y, z)];

        public static implicit operator ReadOnlyChunk(Chunk c)
            => c.ToReadOnlyChunk();

        public ReadOnlyChunk ToReadOnlyChunk()
            => new ReadOnlyChunk(States, Skylight, Blocklight);
    }
}