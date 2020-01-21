using System;
using System.Buffers;
using System.Buffers.Binary;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Frontend
{
    public struct MCPacketWriter : IPacketWriter
    {
        public readonly Memory<byte> Memory;
        public int Position;

        public MCPacketWriter(Memory<byte> mem) : this()
        {
            Memory = mem;
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

            var utf8Span = Memory.Slice(Position, value.Length).Span;

            DownsizeUtf16(value, utf8Span);

            Position += value.Length;
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
                if (Sse2.IsSupported)
                {
                    while ((i + (Vector128<short>.Count * 2)) < utf16.Length)
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
            BinaryPrimitives.WriteUInt16BigEndian(Memory.Slice(Position, sizeof(UInt16)).Span, value);
            Position += sizeof(UInt16);
        }

        public void WriteInt16(Int16 value)
        {
            BinaryPrimitives.WriteInt16BigEndian(Memory.Slice(Position, sizeof(Int16)).Span, value);
            Position += sizeof(Int16);
        }

        public void WriteUInt32(UInt32 value)
        {
            BinaryPrimitives.WriteUInt32BigEndian(Memory.Slice(Position, sizeof(UInt32)).Span, value);
            Position += sizeof(UInt32);
        }

        public void WriteInt32(Int32 value)
        {
            BinaryPrimitives.WriteInt32BigEndian(Memory.Slice(Position, sizeof(Int32)).Span, value);
            Position += sizeof(Int32);
        }

        public void WriteUInt64(UInt64 value)
        {
            BinaryPrimitives.WriteUInt64BigEndian(Memory.Slice(Position, sizeof(UInt64)).Span, value);
            Position += sizeof(UInt64);
        }

        public void WriteInt64(Int64 value)
        {
            BinaryPrimitives.WriteInt64BigEndian(Memory.Slice(Position, sizeof(Int64)).Span, value);
            Position += sizeof(Int64);
        }

        public unsafe void WriteGuid(Guid value)
        {
            // this is what GUID is defined as on Windows (and therefore, from history, in .Net)
            var ptr = (byte*)Unsafe.AsPointer(ref value);
            WriteInt32(Unsafe.Read<int>(ptr));
            ptr += sizeof(int);
            WriteInt16(Unsafe.Read<short>(ptr));
            ptr += sizeof(short);
            WriteInt16(Unsafe.Read<short>(ptr));
            ptr += sizeof(short);
            WriteBytes(new Span<byte>(ptr, 8));
        }

        public void WriteBoolean(bool value)
            => WriteUInt8(value ? (byte)0x01 : (byte)0x00);

        public void WriteSingle(float value)
            => WriteInt32(BitConverter.SingleToInt32Bits(value));

        public void WriteDouble(double value)
            => WriteInt64(BitConverter.DoubleToInt64Bits(value));

        public void WriteNbt(NbtCompound? compound, string name = "")
        {
            using var writer = new NbtWriter();
            if (name != null && compound != null)
            {
                writer.WriteByte(compound.Value.TagType);
                writer.WriteString(name);
            }

            writer.WriteRoot(compound, false);
            var array = writer.Stream.ToArray(); // :/
            WriteBytes(array);
        }

        public void WritePosition(Vector3Int position)
        {
            position.Deconstruct(out int x, out int y, out int z);
            WriteInt64(((((long)x) & 0x3FFFFFF) << 38) | ((((long)z) & 0x3FFFFFF) << 12) | (((long)y) & 0xFFF));
        }

        public void WriteVarInt(int value)
        {
            var size = 0;
            var v = unchecked((uint)value);
            while ((v & -0x80) != 0)
            {
                if (size > 5)
                    throw new IOException("VarInt too long, its just, i can't handle that");

                WriteUInt8((byte)(v & 0x7F | 0x80));
                v >>= 7;
                size++;
            }

            WriteUInt8((byte)v);
        }


    }
}
