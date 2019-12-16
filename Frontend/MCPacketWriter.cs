using System;
using System.Buffers;
using System.Buffers.Binary;
using System.IO;

namespace Frontend
{
    public ref struct MCPacketWriter
    {
        private MemoryPool<byte> _memPool;
        public Span<byte> Span;
        public int Position;

        public MCPacketWriter(MemoryPool<byte> memoryPool) : this()
        {
            _memPool = memoryPool;
        }
        
        public int GetVarIntSize(int val)
        {
            var size = 0;
            var v = val;
            while ((v & -0x80) != 0)
            {
                v = (int)(((uint)v) >> 7);
                size++;
            }

            return size;
        }

        public void WriteByte(byte value) 
            => Span[Position++] = value;

        public void WriteBytes(ReadOnlySpan<byte> values)
        {
            values.CopyTo(Span.Slice(Position));
            Position += values.Length;
        }
        
        public void WriteInt64(Int64 value)
        {
            using var mem = _memPool.Rent(sizeof(Int64));
            var span = mem.Memory.Span;
            BinaryPrimitives.WriteInt64BigEndian(span, value);
            WriteBytes(span);
        }

        public void WriteVarInt(int value)
        {
            var size = 0;
            var v = value;
            while ((v & -0x80) != 0)
            {
                if (size > 5)
                    throw new IOException("VarInt too long, its just, i can't handle that");

                WriteByte((byte)(v & 0x7F | 0x80));
                v = (int)(((uint)v) >> 7);
                size++;
            }

            WriteByte((byte)v);
        }
    }
}