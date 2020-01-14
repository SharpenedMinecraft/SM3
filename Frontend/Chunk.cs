using System;
using System.Diagnostics;

namespace Frontend
{
    public readonly struct Chunk
    {
        public readonly Memory<BlockState> States;
        /// <summary>
        /// Every byte contains two values
        /// </summary>
        public readonly Memory<byte> Skylight;
        /// <summary>
        /// Every byte contains two values
        /// </summary>
        public readonly Memory<byte> Blocklight;
        
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

        // DO NOT CHANGE. LARGE PARTS OF CODE AND OPTIMIZATIONS RELY ON THE X-Z-Y LAYOUT
        public readonly int CalculateStateIndex(BlockPosition position)
            => position.X + Width * (position.Z + Depth * position.Y);

        public readonly int CalculateLightIndex(BlockPosition position)
            => CalculateStateIndex(position) / 2;

        public ref BlockState this[BlockPosition position] => ref States.Span[CalculateStateIndex(position)];

        public static implicit operator ReadOnlyChunk(Chunk c)
            => c.ToReadOnlyChunk();

        public ReadOnlyChunk ToReadOnlyChunk()
            => new ReadOnlyChunk(States, Skylight, Blocklight);
    }
}