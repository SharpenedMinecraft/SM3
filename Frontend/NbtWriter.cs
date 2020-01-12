using System;
using System.Buffers.Binary;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Frontend
{
    public sealed class NbtWriter : IDisposable
    {
        private MemoryStream _stream;

        public NbtWriter()
        {
            _stream = new MemoryStream();
        }

        public void WriteTag(INbtTag tag)
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
        
        private void WriteLongArray(long[] value)
        {
            WriteInt(value.Length);
            
            for (int i = 0; i < value.Length; i++)
                WriteLong(value[i]);
        }


        private void WriteIntArray(int[] value)
        {
            WriteInt(value.Length);
            
            for (int i = 0; i < value.Length; i++)
                WriteInt(value[i]);
        }

        private void WriteCompound(ReadOnlyDictionary<string,INbtTag> value)
        {
            foreach ((string name, INbtTag tag) in value)
            {
                WriteByte(tag.TagType);
                WriteString(name);

                WriteTag(tag);
            }
        }

        private void WriteList(INbtTag[] value)
        {
            WriteByte(value[0].TagType);
            WriteInt(value.Length);

            for (int i = 0; i < value.Length; i++)
            {
                WriteTag(value[i]);
            }
        }

        private void WriteString(string value)
        {
            WriteShort((short)value.Length);
            _stream.Write(Encoding.UTF8.GetBytes(value));
        }

        private void WriteByteArray(byte[] value)
        {
            WriteInt(value.Length);
            _stream.Write(value);
        }

        private void WriteDouble(double value)
            => WriteLong(BitConverter.DoubleToInt64Bits(value));
        
        private void WriteFloat(float value)
            => WriteInt(BitConverter.SingleToInt32Bits(value));

        private void WriteLong(long value)
        {
            Span<byte> b = stackalloc byte[sizeof(long)];
            BinaryPrimitives.WriteInt64BigEndian(b, value);
            _stream.Write(b);
        }

        private void WriteInt(int value)
        {
            Span<byte> b = stackalloc byte[sizeof(int)];
            BinaryPrimitives.WriteInt32BigEndian(b, value);
            _stream.Write(b);
        }

        private void WriteShort(short value)
        {
            Span<byte> b = stackalloc byte[sizeof(short)];
            BinaryPrimitives.WriteInt16BigEndian(b, value);
            _stream.Write(b);
        }

        private void WriteByte(byte value)
        {
            _stream.WriteByte(value);
        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void OutOfRangeTypeCodeThrow() => throw new ArgumentOutOfRangeException("typeCode");

        public void Dispose()
        {
            _stream.Dispose();
        }
    }
}