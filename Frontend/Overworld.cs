using System;
using System.Collections.Generic;

namespace Frontend
{
    public sealed class Overworld : IDimension
    {
        private Dictionary<ChunkPosition, Chunk> _chunks = new Dictionary<ChunkPosition, Chunk>();
        
        public int Id => 0;

        public Chunk? GetChunk(ChunkPosition position)
        {
            if (_chunks.TryGetValue(position, out var v))
                return v;

            return null;
        }

        public Chunk Load(ChunkPosition position)
        {
            if (_chunks.TryGetValue(position, out var v))
                return v;

            Memory<BlockState> blockStates = new BlockState[Chunk.Width * Chunk.Height * Chunk.Depth];
            Memory<byte> skylight = new byte[Chunk.Width * Chunk.Height * Chunk.Depth / 2];
            Memory<byte> blocklight = new byte[Chunk.Width * Chunk.Height * Chunk.Depth / 2];
            
            blockStates.Span.Fill(new BlockState(1));
            skylight.Span.Fill(12);
            
            var chunk = new Chunk(blockStates,
                                  skylight,
                                  blocklight);
            
            _chunks[position] = chunk;
            return chunk;
        }

        public void Unload(ChunkPosition position)
        {
            if (!_chunks.ContainsKey(position))
                return;

            _chunks.Remove(position);
        }

        public IEnumerable<Chunk> GetAllChunks()
            => _chunks.Values;
    }
}