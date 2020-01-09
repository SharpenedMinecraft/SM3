using System;
using System.Buffers;

namespace Frontend
{
    public interface IPacketWriter
    {
        void WriteVarInt(int value);
        void WriteString(ReadOnlySpan<char> value); // TODO: Once UTF-8 String exsists, change this
        void WriteBytes(ReadOnlySpan<byte> value);

        void WriteBoolean(bool value);
        void WriteUInt8(byte value);
        void WriteInt8(sbyte value);
        void WriteUInt16(ushort value);
        void WriteInt16(short value);
        void WriteUInt32(uint value);
        void WriteInt32(int value);
        void WriteUInt64(ulong value);
        void WriteInt64(long value);
        void WriteGuid(Guid value);

        void WriteSingle(float value);
        void WriteDouble(double value);
    }
}