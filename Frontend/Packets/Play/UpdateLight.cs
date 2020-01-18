using System;
using System.Diagnostics;

namespace Frontend.Packets.Play
{
    public readonly struct UpdateLight : IWriteablePacket
    {
        public int Id => 0x25;

        public readonly ChunkPosition Position;
        public readonly Chunk Chunk;

        public UpdateLight(ChunkPosition position, Chunk chunk)
        {
            Position = position;
            Chunk = chunk;
        }

        public void Write(IPacketWriter writer)
        {
            writer.WriteVarInt(Position.X);
            writer.WriteVarInt(Position.Z);

            const int last18Bits = 0b0000_0000_0000_0011_1111_1111_1111_1111;
            int skylightZeroMask = CalculateZeroMask(Chunk.Skylight.Span, Chunk) & last18Bits;
            int blocklightZeroMask = CalculateZeroMask(Chunk.Blocklight.Span, Chunk) & last18Bits;
            
            // note, we always send all light values. In theory, it is allowed to only update specific sections using this mask
            // meaning the zero mask is not necessarily the inverse of the main mask
            var skylightWriteMask = ~skylightZeroMask & last18Bits;
            writer.WriteVarInt(skylightWriteMask);
            var blocklightWriteMask = ~blocklightZeroMask & last18Bits;
            writer.WriteVarInt(blocklightWriteMask);
            
            writer.WriteVarInt(skylightZeroMask);
            writer.WriteVarInt(blocklightZeroMask);
            
            WriteSections(Chunk.Skylight.Span, skylightWriteMask, writer);
            WriteSections(Chunk.Blocklight.Span, blocklightWriteMask, writer);
        }

        private void WriteSections(ReadOnlySpan<byte> light, int writeMask, IPacketWriter writer)
        {
            for (int section = -1; section < 17; section++)
            {
                if (((writeMask >> (section + 1)) & 0b1) == 0)
                    continue;
                
                const int planeSize = Chunk.Width * Chunk.Depth;
                var data = light.Slice((section * 16 * planeSize) / 2, (16 * planeSize) / 2);
                Debug.Assert(data.Length == 2048);
                writer.WriteVarInt(2048);
                writer.WriteBytes(data);
            }
        }

        private int CalculateZeroMask(ReadOnlySpan<byte> light, in ReadOnlyChunk chunk)
        {
            // sections void (lowest bit) and above the world (highest of the 18 bits) are set to 1, because they are ignored.
            int mask = 0b0000_0000_0000_0010_0000_0000_0000_0001;
            
            for (int section = 0; section < 16; section++)
            {
                for (int y = 0; y < 16; y++)
                {
                    for (int x = 0; x < Chunk.Width; x++)
                    {
                        for (int z = 0; z < Chunk.Depth; z++)
                        {
                            var index = chunk.CalculateLightIndex((x, (y + section * 16), z));
                            if (light[index] != 0)
                            {
                                goto nomask;
                            }
                        }
                    }
                }

                mask |= 1 << (section + 1);
                nomask: ;
            }

            return mask;
        }
    }
}