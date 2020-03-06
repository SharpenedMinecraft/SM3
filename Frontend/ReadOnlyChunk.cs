using System;
using System.Diagnostics;

namespace SM3.Frontend
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
        
        // DO NOT CHANGE. LARGE PARTS OF CODE AND OPTIMIZATIONS RELY ON THE X-Z-Y LAYOUT
        public int CalculateStateIndex(BlockPosition position)
            => position.X + Width * (position.Z + Depth * position.Y);

        public int CalculateLightIndex(BlockPosition position)
            => CalculateStateIndex(position) / 2;

        public BlockState this[BlockPosition position] => States.Span[CalculateStateIndex(position)];
    }
}