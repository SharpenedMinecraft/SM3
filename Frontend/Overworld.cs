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

        public void Load(ChunkPosition position)
        {
            if (_chunks.ContainsKey(position))
                return;

            Memory<byte> light = new byte[Chunk.Width * Chunk.Height * Chunk.Depth];
            
            _chunks[position] = new Chunk(new BlockState[Chunk.Width * Chunk.Height * Chunk.Depth],
                                          light,
                                          light.Slice(light.Length / 2));
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