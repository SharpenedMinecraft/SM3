using System;
using System.Buffers;
using System.Buffers.Binary;
using System.IO;
using System.Text;

namespace Frontend
{
    public struct MCPacketReader : IPacketReader
    {
        private ReadOnlySequence<byte> _buffer;

        public ReadOnlySequence<byte> Buffer => _buffer;

        public MCPacketReader(ReadOnlySequence<byte> buffer)
        {
            _buffer = buffer;
        }
        
        public ReadOnlySpan<byte> ReadBytes(int length)
        {
            var data = _buffer.Slice(0, length);
            _buffer = _buffer.Slice(length);

            if (data.IsSingleSegment)
                return data.FirstSpan;
            else
            {
                var v = new Span<byte>(new byte[length]);
                data.CopyTo(v);
                return v;
            }
        }

        public byte ReadUInt8()
        {
            var b = _buffer.FirstSpan[0];
            _buffer = _buffer.Slice(1);
            return b;
        }

        public sbyte ReadInt8()
        {
            unchecked
            {
                return (sbyte)ReadUInt8();
            }
        }

        public ulong ReadUInt64()
            =>BinaryPrimitives.ReadUInt64BigEndian(ReadBytes(sizeof(UInt64)));

        public long ReadInt64() 
            => BinaryPrimitives.ReadInt64BigEndian(ReadBytes(sizeof(Int64)));

        public float ReadSingle()
            => BitConverter.Int32BitsToSingle(ReadInt32());

        public double ReadDouble()
            => BitConverter.Int64BitsToDouble(ReadInt64());

        public ushort ReadUInt16() 
            => BinaryPrimitives.ReadUInt16BigEndian(ReadBytes(sizeof(ushort)));

        public short ReadInt16()
            => BinaryPrimitives.ReadInt16BigEndian(ReadBytes(sizeof(short)));

        public uint ReadUInt32()
            => BinaryPrimitives.ReadUInt32BigEndian(ReadBytes(sizeof(uint)));

        public int ReadInt32()
            => BinaryPrimitives.ReadInt32BigEndian(ReadBytes(sizeof(int)));

        public int ReadVarInt()
        {
            var val = 0;
            var size = 0;
            int readData;
            while (((readData = ReadUInt8()) & 0x80) == 0x80)
            {
                val |= (readData & 0x7F) << (size++ * 7);
                if (size > 5)
                {
                    throw new IOException("VarInt too long");
                }
            }

            return val | ((readData & 0x7F) << (size * 0x7));
        }

        public ReadOnlySpan<char> ReadString()
        {
            var length = ReadVarInt();

            return Encoding.UTF8.GetString(ReadBytes(length));
        }
    }
}