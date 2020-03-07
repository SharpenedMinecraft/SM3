using System;
using System.Numerics;
using System.Text.Json;
using SM3.NBT;

namespace SM3.Network
{
    public sealed class EntityMetadata
    {
        private readonly IPacketWriter _writer;

        public EntityMetadata(IPacketWriter writer)
        {
            _writer = writer;
        }

        public void WriteUInt8(byte index, byte value)
        {
            _writer.WriteUInt8(index);
            _writer.WriteUInt8(0);
            _writer.WriteUInt8(value);
        }

        public void WriteVarInt(byte index, int value)
        {
            _writer.WriteUInt8(index);
            _writer.WriteUInt8(1);
            _writer.WriteVarInt(value);
        }

        public void WriteSingle(byte index, float value)
        {
            _writer.WriteUInt8(index);
            _writer.WriteUInt8(2);
            _writer.WriteSingle(value);
        }

        public void WriteString(byte index, string value)
        {
            _writer.WriteUInt8(index);
            _writer.WriteUInt8(3);
            _writer.WriteString(value);
        }

        public void WriteChat(byte index, Chat value)
        {
            _writer.WriteUInt8(index);
            _writer.WriteUInt8(4);
            var data = JsonSerializer.SerializeToUtf8Bytes(value);
            _writer.WriteVarInt(data.Length);
            _writer.WriteBytes(data);
        }

        public void WriteOptChat(byte index, Chat? value)
        {
            _writer.WriteUInt8(index);
            _writer.WriteUInt8(5);
            var b = value is null;
            _writer.WriteBoolean(!b);

            if (!b)
            {
                var data = JsonSerializer.SerializeToUtf8Bytes(value);
                _writer.WriteVarInt(data.Length);
                _writer.WriteBytes(data);
            }
        }

        public void WriteSlot(byte index, NetworkSlot value)
        {
            _writer.WriteUInt8(index);
            _writer.WriteUInt8(6);
            _writer.WriteSpecialType(value);
        }

        public void WriteBoolean(byte index, bool value)
        {
            _writer.WriteUInt8(index);
            _writer.WriteUInt8(7);
            _writer.WriteBoolean(value);
        }

        public void WriteRotation(byte index, Vector3 value)
        {
            _writer.WriteUInt8(index);
            _writer.WriteUInt8(8);
            _writer.WriteSingle(value.X);
            _writer.WriteSingle(value.Y);
            _writer.WriteSingle(value.Z);
        }

        public void WritePosition(byte index, Vector3Int value)
        {
            _writer.WriteUInt8(index);
            _writer.WriteUInt8(9);
            _writer.WritePosition(value);
        }

        public void WriteOptPosition(byte index, Vector3Int? value)
        {
            _writer.WriteUInt8(index);
            _writer.WriteUInt8(10);
            _writer.WriteBoolean(!(value is null));

            if (!(value is null))
            {
                _writer.WritePosition(value.Value);
            }
        }

        public void WriteDirection(byte index, Direction value)
        {
            _writer.WriteUInt8(index);
            _writer.WriteUInt8(11);
            _writer.WriteVarInt((int)value);
        }

        public void WriteOptGuid(byte index, Guid? value)
        {
            _writer.WriteUInt8(index);
            _writer.WriteUInt8(12);
            _writer.WriteBoolean(!(value is null));

            if (!(value is null))
            {
                _writer.WriteGuid(value.Value);
            }
        }

        public void WriteOptBlockId(byte index, BlockState value)
        {
            _writer.WriteUInt8(index);
            _writer.WriteUInt8(13);
            _writer.WriteVarInt(value.State);
        }

        public void WriteNbt(byte index, NbtCompound value)
        {
            _writer.WriteUInt8(index);
            _writer.WriteUInt8(14);
            _writer.WriteNbt(value);
        }

        public void WriteVillagerData(byte index, int type, int profession, int level)
        {
            _writer.WriteUInt8(index);
            _writer.WriteUInt8(15);
            _writer.WriteVarInt(type);
            _writer.WriteVarInt(profession);
            _writer.WriteVarInt(level);
        }

        public void WriteOptVarInt(byte index, int? value)
        {
            _writer.WriteUInt8(index);
            _writer.WriteUInt8(16);
            _writer.WriteVarInt(value ?? 0);
        }

        public void WritePose(byte index, int pose)
        {
            _writer.WriteUInt8(index);
            _writer.WriteUInt8(17);
            _writer.WriteVarInt(pose);
        }

        public void WriteEnd()
        {
            _writer.WriteUInt8(0xFF);
        }
    }
}
