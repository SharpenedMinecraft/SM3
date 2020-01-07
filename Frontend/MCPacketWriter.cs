using System;
using System.Buffers;
using System.Buffers.Binary;
using System.IO;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Frontend
{
    public struct MCPacketWriter : IPacketWriter
    {
        private readonly MemoryPool<byte> _memPool;
        public readonly Memory<byte> Memory;
        public int Position;

        public MCPacketWriter(Memory<byte> mem, MemoryPool<byte> memoryPool) : this()
        {
            Memory = mem;
            _memPool = memoryPool;
        }

        public void WriteUInt8(byte value)
            => Memory.Span[Position++] = value;

        public void WriteInt8(sbyte value)
            => WriteUInt8((byte) value);

        public unsafe void WriteString(ReadOnlySpan<char> value)
        {
            WriteVarInt(value.Length);
            // This code could get a lot of help from Char8.
            // for now, we simply drop every second byte.

            using var utf8Owner = _memPool.Rent(value.Length);
            var utf8Span = utf8Owner.Memory.Span.Slice(0, value.Length);

            DownsizeUtf16(value, utf8Span);
            
            WriteBytes(utf8Span);
        }

        public static unsafe void DownsizeUtf16(ReadOnlySpan<char> utf16, Span<byte> utf8)
        {
            int i = 0;

            fixed (char* pUtf16Char = utf16)
            fixed (byte* pUtf8 = utf8)
            {
                // HACK: (?) assuming sizeof(char) == sizeof(short)
                var pUtf16 = (short*) pUtf16Char;
#if !NO_OPTIMIZATION
#if AVX // TAKE CARE. AVX MAY CAUSE DOWNCLOCKING
                /*if (Avx2.IsSupported)
                {
                    while ((i + Vector256<short>.Count * 2) < value.Length)
                    {
                        var vector1 = Avx2.LoadVector256(pUtf16 + i);
                        var vector2 = Avx2.LoadVector256(pUtf16 + i + Vector256<short>.Count);
                        
                        Avx2.Store(pUtf8 + i, Avx2.PackUnsignedSaturate(vector1, vector2));
                        i += Vector256<short>.Count * 2;
                    }
                }*/
#endif

                if (Sse2.IsSupported)
                {
                    
                    while ((i + Vector128<short>.Count * 2) < utf16.Length)
                    {
                        var vector1 = Sse2.LoadVector128(pUtf16 + i);
                        var vector2 = Sse2.LoadVector128(pUtf16 + i + Vector128<short>.Count);
                        
                        Sse2.Store(pUtf8 + i, Sse2.PackUnsignedSaturate(vector1, vector2));
                        i += Vector128<short>.Count * 2;
                    }
                }
#endif
                while (i < utf16.Length)
                {
                    utf8[i] = *(byte*)(pUtf16 + i);
                    i += 1;
                }
            }
        }

        public void WriteBytes(ReadOnlySpan<byte> values)
        {
            values.CopyTo(Memory.Slice(Position).Span);
            Position += values.Length;
            
#if DUMP_WRITE_BYTES
            Console.WriteLine($"Wrote {values.Length}:");
            Console.WriteLine(BitConverter.ToString(values.ToArray()));
#endif
        }
        
        public void WriteUInt16(UInt16 value)
        {
            using var mem = _memPool.Rent(sizeof(UInt16));
            var span = mem.Memory.Span.Slice(0, sizeof(UInt16));
            BinaryPrimitives.WriteUInt16BigEndian(span, value);
            WriteBytes(span);
        }

        public void WriteInt16(Int16 value)
        {
            using var mem = _memPool.Rent(sizeof(Int16));
            var span = mem.Memory.Span.Slice(0, sizeof(Int16));
            BinaryPrimitives.WriteInt16BigEndian(span, value);
            WriteBytes(span);
        }
        
        public void WriteUInt32(UInt32 value)
        {
            using var mem = _memPool.Rent(sizeof(UInt32));
            var span = mem.Memory.Span.Slice(0, sizeof(UInt32));
            BinaryPrimitives.WriteUInt32BigEndian(span, value);
            WriteBytes(span);
        }
        
        public void WriteInt32(Int32 value)
        {
            using var mem = _memPool.Rent(sizeof(Int32));
            var span = mem.Memory.Span.Slice(0, sizeof(Int32));
            BinaryPrimitives.WriteInt32BigEndian(span, value);
            WriteBytes(span);
        }
        
        public void WriteUInt64(UInt64 value)
        {
            using var mem = _memPool.Rent(sizeof(UInt64));
            var span = mem.Memory.Span.Slice(0, sizeof(UInt64));
            BinaryPrimitives.WriteUInt64BigEndian(span, value);
            WriteBytes(span);
        }
        
        public void WriteInt64(Int64 value)
        {
            using var mem = _memPool.Rent(sizeof(Int64));
            var span = mem.Memory.Span.Slice(0, sizeof(Int64));
            BinaryPrimitives.WriteInt64BigEndian(span, value);
            WriteBytes(span);
        }

        public void WriteBoolean(bool value)
            => WriteUInt8(value ? (byte)0x01 : (byte)0x00);

        public void WriteSingle(float value) 
            => WriteInt32(BitConverter.SingleToInt32Bits(value));

        public void WriteDouble(double value)
            => WriteInt64(BitConverter.DoubleToInt64Bits(value));

        public void WriteVarInt(int value)
        {
            var size = 0;
            var v = value;
            while ((v & -0x80) != 0)
            {
                if (size > 5)
                    throw new IOException("VarInt too long, its just, i can't handle that");

                WriteUInt8((byte)(v & 0x7F | 0x80));
                v = (int)(((uint)v) >> 7);
                size++;
            }

            WriteUInt8((byte)v);
        }
    }
}