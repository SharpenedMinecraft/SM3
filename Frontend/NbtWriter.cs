using System;
using System.Buffers.Binary;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace SM3.Frontend
{
    public sealed class NbtWriter : IDisposable
    {
        public MemoryStream Stream { get; }

        public NbtWriter()
        {
            Stream = new MemoryStream();
        }

        public void WriteRoot(NbtCompound? root, bool isImplicit = true)
        {
            if (root != null)
                WriteCompound(root.Value.Value, isImplicit);
            else if (!isImplicit)
                WriteByte(0);
        }

        public void WriteRoot(NbtCompound root)
        {
            WriteCompound(root.Value, true);
        }

        private void WriteTag(INbtTag tag)
        {
            switch (tag.TagType)
            {
                case 1:
                    WriteByte(((NbtByte) tag).Value);
                    break;
                case 2:
                    WriteShort(((NbtShort) tag).Value);
                    break;
                case 3:
                    WriteInt(((NbtInt) tag).Value);
                    break;
                case 4:
                    WriteLong(((NbtLong) tag).Value);
                    break;
                case 5:
                    WriteFloat(((NbtFloat) tag).Value);
                    break;
                case 6:
                    WriteDouble(((NbtFloat) tag).Value);
                    break;
                case 7:
                    WriteByteArray(((NbtByteArray) tag).Value);
                    break;
                case 8:
                    WriteString(((NbtString) tag).Value);
                    break;
                case 9:
                    WriteList(((NbtList) tag).Value);
                    break;
                case 10:
                    WriteCompound(((NbtCompound) tag).Value);
                    break;
                case 11:
                    WriteIntArray(((NbtIntArray) tag).Value);
                    break;
                case 12:
                    WriteLongArray(((NbtLongArray) tag).Value);
                    break;

                default:
                    OutOfRangeTypeCodeThrow();
                    break;
            }
        }

        public void WriteLongArray(long[] value)
        {
            WriteInt(value.Length);

            for (int i = 0; i < value.Length; i++)
                WriteLong(value[i]);
        }


        public void WriteIntArray(int[] value)
        {
            WriteInt(value.Length);

            for (int i = 0; i < value.Length; i++)
                WriteInt(value[i]);
        }

        public void WriteCompound(ReadOnlyDictionary<string,INbtTag> value, bool isImplicit = false)
        {
            foreach ((string name, INbtTag tag) in value)
            {
                WriteByte(tag.TagType);
                WriteString(name);

                WriteTag(tag);
            }

            if (!isImplicit)
                WriteByte(0);
        }

        public void WriteList(INbtTag[] value)
        {
            WriteByte(value[0].TagType);
            WriteInt(value.Length);

            for (int i = 0; i < value.Length; i++)
            {
                WriteTag(value[i]);
            }
        }

        public void WriteString(string value)
        {
            WriteShort((short)value.Length);
            Stream.Write(Encoding.UTF8.GetBytes(value));
        }

        public void WriteByteArray(byte[] value)
        {
            WriteInt(value.Length);
            Stream.Write(value);
        }

        public void WriteDouble(double value)
            => WriteLong(BitConverter.DoubleToInt64Bits(value));

        public void WriteFloat(float value)
            => WriteInt(BitConverter.SingleToInt32Bits(value));

        public void WriteLong(long value)
        {
            Span<byte> b = stackalloc byte[sizeof(long)];
            BinaryPrimitives.WriteInt64BigEndian(b, value);
            Stream.Write(b);
        }

        public void WriteInt(int value)
        {
            Span<byte> b = stackalloc byte[sizeof(int)];
            BinaryPrimitives.WriteInt32BigEndian(b, value);
            Stream.Write(b);
        }

        public void WriteShort(short value)
        {
            Span<byte> b = stackalloc byte[sizeof(short)];
            BinaryPrimitives.WriteInt16BigEndian(b, value);
            Stream.Write(b);
        }

        public void WriteByte(byte value)
        {
            Stream.WriteByte(value);
        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void OutOfRangeTypeCodeThrow() => throw new ArgumentOutOfRangeException("typeCode");

        public void Dispose()
        {
            Stream.Dispose();
        }
    }
}
