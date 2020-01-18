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
            
            
            var chunk = new Chunk(blockStates,
                                  skylight,
                                  blocklight);
            
            GenerateChunk(chunk);
            
            _chunks[position] = chunk;
            return chunk;
        }

        private void GenerateChunk(in Chunk chunk)
        {
            var blocklightSpan = chunk.Blocklight.Span;
            var skylightSpan = chunk.Skylight.Span;
            var statesSpan = chunk.States.Span;
            
            for (int x = 0; x < Chunk.Width; x++)
            {
                for (int z = 0; z < Chunk.Depth; z++)
                {
                    for (int y = 0; y < Chunk.Height; y++)
                    {
                        var pos = new BlockPosition(x, y, z);
                        var blockIndex = chunk.CalculateStateIndex(pos);
                        var lightIndex = chunk.CalculateLightIndex(pos);

                        BlockState state;
                        byte skyLight;
                        byte blockLight;
                        
                        switch (y)
                        {
                            case 0:
                                state = new BlockState(33);
                                blockLight = 0;
                                skyLight = 0;
                                break;
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                                state = new BlockState(1);
                                blockLight = 0;
                                skyLight = 0;
                                break;
                            
                            case 6:
                            case 7:
                            case 8:
                                state = new BlockState(10);
                                blockLight = 0;
                                skyLight = 0;
                                break;
                            case 9:
                                state = new BlockState(9);
                                blockLight = 12;
                                skyLight = 12;
                                break;
                            default:
                                state = new BlockState(0);
                                blockLight = 12;
                                skyLight = 12;
                                break;
                        }
                        
                        blocklightSpan[lightIndex] = blockLight;
                        skylightSpan[lightIndex] = skyLight;
                        statesSpan[blockIndex] = state;
                    }
                }
            }
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