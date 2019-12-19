using System;

namespace Frontend
{
    public interface IPacketReader
    {
        int ReadVarInt();
        ReadOnlySpan<char> ReadString(); // TODO: Once UTF-8 String exsists, change this
        ReadOnlySpan<byte> ReadBytes(int length);

        byte ReadUInt8();
        sbyte ReadInt8();
        ushort ReadUInt16();
        short ReadInt16();
        uint ReadUInt32();
        int ReadInt32();
        ulong ReadUInt64();
        long ReadInt64();

        float ReadSingle();
        double ReadDouble();
    }
}