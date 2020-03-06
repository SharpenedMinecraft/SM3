using System.Collections.ObjectModel;

namespace SM3.Frontend
{
    public interface INbtTag
    {
        string TagTypeName { get; }
        byte TagType { get; }
    }

    public readonly struct NbtByte : INbtTag
    {
        public string TagTypeName => "TAG_Byte";
        public byte TagType => 1;
        
        public readonly byte Value;

        public NbtByte(byte value)
        {
            Value = value;
        }
    }

    public readonly struct NbtShort : INbtTag
    {
        public string TagTypeName => "TAG_Short";
        public byte TagType => 2;
        
        public readonly short Value;

        public NbtShort(short value)
        {
            Value = value;
        }
    }

    public readonly struct NbtInt : INbtTag
    {
        public string TagTypeName => "TAG_Int";
        public byte TagType => 3;
        
        public readonly int Value;

        public NbtInt(int value)
        {
            Value = value;
        }
    }

    public readonly struct NbtLong : INbtTag
    {
        public string TagTypeName => "TAG_Long";
        public byte TagType => 4;
        
        public readonly long Value;

        public NbtLong(long value)
        {
            Value = value;
        }
    }

    public readonly struct NbtFloat : INbtTag
    {
        public string TagTypeName => "TAG_Float";
        public byte TagType => 5;
        
        public readonly float Value;

        public NbtFloat(float value)
        {
            Value = value;
        }
    }

    public readonly struct NbtDouble : INbtTag
    {
        public string TagTypeName => "TAG_Double";
        public byte TagType => 6;
        
        public readonly double Value;

        public NbtDouble(double value)
        {
            Value = value;
        }
    }

    public readonly struct NbtByteArray : INbtTag
    {
        public string TagTypeName => "TAG_Byte_Array";
        public byte TagType => 7;
        
        public readonly byte[] Value;

        public NbtByteArray(byte[] value)
        {
            Value = value;
        }
    }

    public readonly struct NbtIntArray : INbtTag
    {
        public string TagTypeName => "TAG_Int_Array";
        public byte TagType => 11;
        
        public readonly int[] Value;

        public NbtIntArray(int[] value)
        {
            Value = value;
        }
    }

    public readonly struct NbtLongArray : INbtTag
    {
        public string TagTypeName => "TAG_Long_Array";
        public byte TagType => 12;
        
        public readonly long[] Value;

        public NbtLongArray(long[] value)
        {
            Value = value;
        }
    }

    public readonly struct NbtString : INbtTag
    {
        public string TagTypeName => "TAG_String";
        public byte TagType => 8;
        
        public readonly string Value;

        public NbtString(string value)
        {
            Value = value;
        }
    }

    public readonly struct NbtCompound : INbtTag
    {
        public string TagTypeName => "TAG_Compound";
        public byte TagType => 10;
        
        public readonly ReadOnlyDictionary<string, INbtTag> Value;

        public NbtCompound(ReadOnlyDictionary<string, INbtTag> value)
        {
            Value = value;
        }
    }

    public readonly struct NbtList : INbtTag
    {
        public string TagTypeName => "TAG_List";
        public byte TagType => 9;
        
        public readonly INbtTag[] Value;

        public NbtList(INbtTag[] value)
        {
            Value = value;
        }
    }
}