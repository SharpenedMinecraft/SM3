using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SM3.NBT
{
    public ref struct NbtReader
    {
        public readonly ReadOnlySpan<byte> Data;
        public int Position;

        public NbtReader(ReadOnlySpan<byte> data)
        {
            Data = data;
            Position = 0;
        }

        public byte ReadByte()
        {
            var b = Data[Position];
            Position++;
            return b;
        }

        public short ReadShort()
        {
            var b = BinaryPrimitives.ReadInt16BigEndian(Data.Slice(Position));
            Position += sizeof(short);
            return b;
        }

        public int ReadInt()
        {
            var b = BinaryPrimitives.ReadInt32BigEndian(Data.Slice(Position));
            Position += sizeof(int);
            return b;
        }

        public long ReadLong()
        {
            var b = BinaryPrimitives.ReadInt64BigEndian(Data.Slice(Position));
            Position += sizeof(long);
            return b;
        }

        public float ReadFloat() => BitConverter.Int32BitsToSingle(ReadInt());

        public double ReadDouble() => BitConverter.Int64BitsToDouble(ReadLong());

        public byte[] ReadByteArray(int length)
        {
            var b = Data.Slice(Position, length).ToArray();
            Position += length;
            return b;
        }

        public string ReadString(int length)
        {
            var b = Encoding.UTF8.GetString(Data.Slice(Position, length));
            Position += length;
            return b;
        }
        
        private int[] ReadIntArray(int length)
        {
            var res = new int[length];
            for (int i = 0; i < length; i++)
            {
                res[i] = ReadInt();
            }

            return res;
        }
        
        private long[] ReadLongArray(int length)
        {
            var res = new long[length];
            for (int i = 0; i < length; i++)
            {
                res[i] = ReadLong();
            }

            return res;
        }
        
        public INbtTag ReadTag(byte typeCode) => typeCode switch
        {
            // 0 => new NbtEnd(),
            1 => new NbtByte(ReadByte()),
            2 => new NbtShort(ReadShort()),
            3 => new NbtInt(ReadInt()),
            4 => new NbtLong(ReadLong()),
            5 => new NbtFloat(ReadFloat()),
            6 => new NbtDouble(ReadDouble()),
            7 => new NbtByteArray(ReadByteArray(ReadInt())),
            8 => new NbtString(ReadString(ReadShort())),
            9 => new NbtList(ReadNbtTagList()),
            10 => ReadCompound(),
            11 => new NbtIntArray(ReadIntArray(ReadInt())),
            12 => new NbtLongArray(ReadLongArray(ReadInt())),
            _ => OutOfRangeTypeCodeThrow()
        };

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static INbtTag OutOfRangeTypeCodeThrow() => throw new ArgumentOutOfRangeException("typeCode");

        public NbtCompound ReadCompound()
        {
            var tags = new Dictionary<string, INbtTag>();

            while (Position < Data.Length) // implicit ending
            {
                var typeCode = ReadByte();
                if (typeCode == 0)
                    break;

                var name = ReadString(ReadShort());
                tags[name] = ReadTag(typeCode);
            }
            
            return new NbtCompound(new ReadOnlyDictionary<string, INbtTag>(tags));
        }

        public INbtTag[] ReadNbtTagList()
        {
            var type = ReadByte();
            var length = ReadInt();

            if (length <= 0)
                return Array.Empty<INbtTag>();

            var res = new INbtTag[length];

            for (int i = 0; i < length; i++)
            {
                res[i] = ReadTag(type);
            }

            return res;
        }
    }
}