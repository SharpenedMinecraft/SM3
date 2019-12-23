using System;
using System.Buffers;
using System.Buffers.Binary;
using System.IO;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
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
        
        public static int GetVarIntSize(int val)
        {
            var size = 0;
            var v = val;
            while ((v & -0x80) != 0)
            {
                v = (int)(((uint)v) >> 7);
                size++;
            }

            return size + 1;
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
            int i = 0;

            fixed (char* pUtf16Char = value)
            fixed (byte* pUtf8 = utf8Span)
            {
                // HACK: (?) assuming sizeof(char) == 2
                // also assuming Vector128<byte>.Count == 16
                // also assuming Vector256<byte>.Count == 32
                var pUtf16 = (byte*) pUtf16Char;
#if !NO_OPTIMIZATION
#if AVX // TAKE CARE. AVX MAY CAUSE DOWNCLOCKING
                if (Avx2.IsSupported)
                {
                    while ((i + 64) < value.Length)
                    {
                        var vector1 = Avx2.LoadVector256(pUtf16 + i);
                        i += 32;
                        var vector2 = Avx2.LoadVector256(pUtf16 + i);
                        i += 32;
                        
                        Avx2.Store(pUtf8 + (i / 2), Avx2.PackUnsignedSaturate(vector1.AsInt16(), vector2.AsInt16()));
                    }
                }
#endif

                if (Sse2.IsSupported)
                {
                    
                    while ((i + 32) < value.Length)
                    {
                        var vector1 = Sse2.LoadVector128(pUtf16 + i);
                        i += 16;
                        var vector2 = Sse2.LoadVector128(pUtf16 + i);
                        i += 16;

                        Sse2.Store(pUtf8 + (i / 2), Sse2.PackUnsignedSaturate(vector1.AsInt16(), vector2.AsInt16()));
                    }
                }
#endif
                while (i < value.Length)
                {
                    utf8Span[i / 2] = *(pUtf16 + i);
                    i += 2;
                }
            }
            WriteBytes(utf8Span);
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
            var span = mem.Memory.Span;
            BinaryPrimitives.WriteUInt16BigEndian(span, value);
            WriteBytes(span);
        }

        public void WriteInt16(Int16 value)
        {
            using var mem = _memPool.Rent(sizeof(Int16));
            var span = mem.Memory.Span;
            BinaryPrimitives.WriteInt16BigEndian(span, value);
            WriteBytes(span);
        }
        
        public void WriteUInt32(UInt32 value)
        {
            using var mem = _memPool.Rent(sizeof(UInt32));
            var span = mem.Memory.Span;
            BinaryPrimitives.WriteUInt32BigEndian(span, value);
            WriteBytes(span);
        }
        
        public void WriteInt32(Int32 value)
        {
            using var mem = _memPool.Rent(sizeof(Int32));
            var span = mem.Memory.Span;
            BinaryPrimitives.WriteInt32BigEndian(span, value);
            WriteBytes(span);
        }
        
        public void WriteUInt64(UInt64 value)
        {
            using var mem = _memPool.Rent(sizeof(UInt64));
            var span = mem.Memory.Span;
            BinaryPrimitives.WriteUInt64BigEndian(span, value);
            WriteBytes(span);
        }
        
        public void WriteInt64(Int64 value)
        {
            using var mem = _memPool.Rent(sizeof(Int64));
            var span = mem.Memory.Span;
            BinaryPrimitives.WriteInt64BigEndian(span, value);
            WriteBytes(span);
        }

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