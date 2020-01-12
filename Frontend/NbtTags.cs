using System.Collections.ObjectModel;

namespace Frontend
{
    public interface INbtTag
    {
        
    }

    public readonly struct NbtByte : INbtTag
    {
        public readonly byte Value;

        public NbtByte(byte value)
        {
            Value = value;
        }
    }

    public readonly struct NbtShort : INbtTag
    {
        public readonly short Value;

        public NbtShort(short value)
        {
            Value = value;
        }
    }

    public readonly struct NbtInt : INbtTag
    {
        public readonly int Value;

        public NbtInt(int value)
        {
            Value = value;
        }
    }

    public readonly struct NbtLong : INbtTag
    {
        public readonly long Value;

        public NbtLong(long value)
        {
            Value = value;
        }
    }

    public readonly struct NbtFloat : INbtTag
    {
        public readonly float Value;

        public NbtFloat(float value)
        {
            Value = value;
        }
    }

    public readonly struct NbtDouble : INbtTag
    {
        public readonly double Value;

        public NbtDouble(double value)
        {
            Value = value;
        }
    }

    public readonly struct NbtByteArray : INbtTag
    {
        public readonly byte[] Value;

        public NbtByteArray(byte[] value)
        {
            Value = value;
        }
    }

    public readonly struct NbtIntArray : INbtTag
    {
        public readonly int[] Value;

        public NbtIntArray(int[] value)
        {
            Value = value;
        }
    }

    public readonly struct NbtLongArray : INbtTag
    {
        public readonly long[] Value;

        public NbtLongArray(long[] value)
        {
            Value = value;
        }
    }

    public readonly struct NbtString : INbtTag
    {
        public readonly string Value;

        public NbtString(string value)
        {
            Value = value;
        }
    }

    public readonly struct NbtCompound : INbtTag
    {
        public readonly ReadOnlyDictionary<string, INbtTag> Value;

        public NbtCompound(ReadOnlyDictionary<string, INbtTag> value)
        {
            Value = value;
        }
    }

    public readonly struct NbtList : INbtTag
    {
        public readonly INbtTag[] Value;

        public NbtList(INbtTag[] value)
        {
            Value = value;
        }
    }
}