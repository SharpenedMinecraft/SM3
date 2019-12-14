using System;
using System.Buffers;
using System.Buffers.Binary;
using System.IO;
using System.Text;

namespace Frontend
{
    public ref struct MCPacketReader
    {
        private ReadOnlySequence<byte> _buffer;

        public ReadOnlySequence<byte> Buffer => _buffer;

        public MCPacketReader(ReadOnlySequence<byte> buffer)
        {
            _buffer = buffer;
        }

        public byte ReadByte()
        {
            var b = _buffer.FirstSpan[0];
            _buffer = _buffer.Slice(1);
            return b;
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

        public UInt16 ReadUInt16() 
            => BinaryPrimitives.ReadUInt16BigEndian(ReadBytes(2));

        public int ReadVarInt()
        {
            var val = 0;
            var size = 0;
            int readData;
            while (((readData = ReadByte()) & 0x80) == 0x80)
            {
                val |= (readData & 0x7F) << (size++ * 7);
                if (size > 5)
                {
                    throw new IOException("VarInt too long");
                }
            }

            return val | ((readData & 0x7F) << (size * 0x7));
        }

        public string ReadString()
        {
            var length = ReadVarInt();

            return Encoding.UTF8.GetString(ReadBytes(length));
        }
    }
}