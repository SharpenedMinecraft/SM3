using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace SM3.Frontend.Packets.Play
{
    public readonly struct ChunkData : IWriteablePacket
    {
        public int Id => 0x22;

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

            var sectionMask = CalculateMask(Chunk.States.Span, Chunk, out var chunksSend, out var chunkNonZeroCount);
            writer.WriteVarInt(sectionMask);

            var rawHeightmap = CalculateHeightmap(Chunk);
            var packedHeightMap = PackHeightMap(rawHeightmap);
            var compound = new NbtCompound(new ReadOnlyDictionary<string, INbtTag>(new Dictionary<string, INbtTag>()
            {
                {"MOTION_BLOCKING", new NbtLongArray(packedHeightMap)}
            }));
            
            const int bitsPerBlock = 14;
            const int requiredLongs = 16 * 16 * 16 * bitsPerBlock / 8 / sizeof(long);
            
            writer.WriteNbt(compound);

            WriteBiomes(writer);
            
            writer.WriteVarInt((chunksSend * (sizeof(short) + sizeof(byte) + 2 + (requiredLongs * sizeof(long)))));

            for (int section = 0; section < 16; section++)
            {
                if ((sectionMask & (1 << section)) == 0)
                    continue;

                writer.WriteInt16(chunkNonZeroCount[section]);
                
                // log2(maxState) rounded. Direct Pallet.
                writer.WriteUInt8(bitsPerBlock);
                writer.WriteVarInt(requiredLongs);
                Span<ulong> longs = new ulong[requiredLongs];
                var bitBuffer = new BitBuffer(longs);

                for (int y = 0; y < 16; y++)
                for (int x = 0; x < ReadOnlyChunk.Width; x++)
                for (int z = 0; z < ReadOnlyChunk.Depth; z++)
                {
                    bitBuffer.WriteInt32(Chunk[(x, (section * 16) + y, z)].State, 14);
                }

                for (int i = 0; i < longs.Length; i++)
                {
                    writer.WriteUInt64(longs[i]);
                }
            }
            
            // Block Entities
            writer.WriteVarInt(0);
        }

        private void WriteBiomes(IPacketWriter writer)
        {
            const int HORIZONTAL_SECTION_COUNT = 2; // (int)Math.round(Math.log(16.0D) / Math.log(2.0D)) - 2;
            const int VERTICAL_SECTION_COUNT = 6; // (int)Math.round(Math.log(256.0D) / Math.log(2.0D)) - 2;
            const int DEFAULT_LENGTH = 1 << HORIZONTAL_SECTION_COUNT + HORIZONTAL_SECTION_COUNT + VERTICAL_SECTION_COUNT;
            // const int HORIZONTAL_BIT_MASK = (1 << HORIZONTAL_SECTION_COUNT) - 1;
            // const int VERTICAL_BIT_MASK = (1 << VERTICAL_SECTION_COUNT) - 1;

            for (int i = 0; i < DEFAULT_LENGTH; i++)
            {
                writer.WriteInt32(127); // void
            }
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

        private int CalculateMask(ReadOnlySpan<BlockState> blocks, in ReadOnlyChunk chunk, out int bitsSet, out short[] nonZeroCount)
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
                            var index = chunk.CalculateStateIndex((x, (y + section * 16), z));
                            if (blocks[index].State != 0)
                            {
                                nonZeroCount[section]++;
                            }
                        }
                    }
                }

                if (nonZeroCount[section] > 0)
                {
                    mask |= 1 << section;
                    bitsSet++;
                }
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