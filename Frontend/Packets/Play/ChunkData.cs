using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Frontend.Packets.Play
{
    public readonly struct ChunkData : IWriteablePacket
    {
        public int Id => 0x21;

        public readonly ChunkPosition Position;
        public readonly ReadOnlyChunk Chunk;

        public ChunkData(ChunkPosition position, ReadOnlyChunk chunk)
        {
            Position = position;
            Chunk = chunk;
        }

        public void Write(IPacketWriter writer)
        {
            writer.WriteInt32(Position.X);
            writer.WriteInt32(Position.Z);
            writer.WriteBoolean(true); // Full Chunks only

            const int last16Bits = 0b0000_0000_0000_0000_1111_1111_1111_1111;
            var zeroSectionMask = CalculateZeroMask(Chunk.States.Span, Chunk, out var chunksNotSend, out var chunkNonZeroCount);
            writer.WriteVarInt((~zeroSectionMask) & last16Bits);

            var rawHeightmap = CalculateHeightmap(Chunk);
            var packedHeightMap = PackHeightMap(rawHeightmap);
            var compound = new NbtCompound(new ReadOnlyDictionary<string, INbtTag>(new Dictionary<string, INbtTag>()
            {
                {"MOTION_BLOCKING", new NbtLongArray(packedHeightMap)}
            }));
            writer.WriteNbt(compound);
            writer.WriteVarInt((16 - chunksNotSend) * (16 * 16 * 16) + (256 * 4));

            for (int section = 0; section < 16; section++)
            {
                if ((zeroSectionMask | (1 << section)) != 0)
                    continue;

                writer.WriteInt16(chunkNonZeroCount[section]);
                
                // log2(maxState) rounded. Direct Pallet.
                const int bitsPerBlock = 14;
                writer.WriteUInt8(bitsPerBlock);
                const int requiredLongs = 16 * 16 * 16 * bitsPerBlock / 8 / sizeof(long);
                writer.WriteVarInt(requiredLongs);
                Span<long> longs = new long[requiredLongs];
                var bitBuffer = new BitBuffer(MemoryMarshal.Cast<long, ulong>(longs));

                for (int x = 0; x < ReadOnlyChunk.Width; x++)
                {
                    for (int z = 0; z < ReadOnlyChunk.Depth; z++)
                    {
                        for (int y = 0; y < 16; y++)
                        {
                            bitBuffer.WriteInt32(Chunk[(x, (section * 16) + y, z)].State, 14);
                        }
                    }
                }
            }

            
            for (int x = 0; x < 16; x++)
            {
                for (int z = 0; z < 16; z++)
                {
                    writer.WriteInt32(127); // Void
                }
            }
            
            // Block Entities
            writer.WriteVarInt(0);
        }

        private int[] CalculateHeightmap(in ReadOnlyChunk chunk)
        {
            var mask = new int[256];
            for (int x = 0; x < ReadOnlyChunk.Width; x++)
            {
                for (int z = 0; z < ReadOnlyChunk.Depth; z++)
                {
                    int y = ReadOnlyChunk.Height - 1;
                    for (; y >= 0; y--)
                    {
                        if (chunk[(x, y, z)].State != 0) // Non-solids should not pass this check
                        {
                            break;
                        }
                    }

                    mask[x + ReadOnlyChunk.Width * z] = y + 1;
                }
            }

            return mask;
        }

        private int CalculateZeroMask(ReadOnlySpan<BlockState> blocks, in ReadOnlyChunk chunk, out int bitsSet, out short[] nonZeroCount)
        {
            bitsSet = 0;
            int mask = 0;
            nonZeroCount = new short[16];


            for (int section = 0; section < 16; section++)
            {
                for (int y = 0; y < 16; y++)
                {
                    for (int x = 0; x < ReadOnlyChunk.Width; x++)
                    {
                        for (int z = 0; z < ReadOnlyChunk.Depth; z++)
                        {
                            var index = chunk.CalculateLightIndex((x, (y + section * 16), z));
                            if (blocks[index].State != 0)
                            {
                                goto nomask;
                            }
                            nonZeroCount[section]++;
                        }
                    }
                }

                mask |= 1 << section;
                bitsSet++;
                nomask: ;
            }

            return mask;
        }

        private long[] PackHeightMap(int[] rawHeightmap)
        {
            var result = new long[36];
            // consider only lowest 9 bits of the raw height
            // and stitch them together
            var bitBuffer = new BitBuffer(MemoryMarshal.Cast<long, ulong>(result));

            for (int i = 0; i < 256; i++)
            {
                bitBuffer.WriteInt32(rawHeightmap[i], 9);
            }
            
            return result;
        }
    }
}