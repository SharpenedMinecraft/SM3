using System;

namespace Frontend
{
  public ref struct BitBuffer
  {
    private const int Bitcount = 64;
    private const int Usedmask = Bitcount - 1;
    private const int Indexshift = 6;
    private const ulong Maxvalue = ulong.MaxValue;

    Span<ulong> _buffer;

    public int BytesRequired => Offset / 8;

    public int Offset { get; set; }

    public int Capacity { get; }

    public bool Overflow => Offset > Capacity;

    public BitBuffer(Span<ulong> span)
    {
      _buffer = span;
      Offset = 0;
      Capacity = span.Length * sizeof(ulong) * 8;
    }

    public bool WriteBoolean(bool value)
    {
      Write(value ? 1UL : 0UL, 1);
      return value;
    }

    public bool ReadBoolean()
    {
      return Read(1) == 1;
    }

    public void WriteInt32(int value, int bits = 32)
    {
      Write(unchecked((ulong) value), bits);
    }

    public int ReadInt32(int bits = 32)
    {
      return unchecked((int) Read(bits));
    }

    public void WriteSingle(float value)
    {
      Write((uint) BitConverter.SingleToInt32Bits(value), 32);
    }

    public float ReadSingle()
    {
      var value = Read(32);
      return BitConverter.Int32BitsToSingle((int) (uint) value);
    }

    void Write(ulong value, int bits)
    {
      if (bits <= 0)
      {
        return;
      }

      value = (value & (Maxvalue >> (Bitcount - bits)));

      int p = Offset >> Indexshift;
      int bitsUsed = Offset & Usedmask;
      int bitsFree = Bitcount - bitsUsed;
      int bitsLeft = bitsFree - bits;
      var d = _buffer;

      if (bitsLeft >= 0)
      {
        ulong mask = (Maxvalue >> bitsFree) | (Maxvalue << (Bitcount - bitsLeft));
        d[p] = ((d[p] & mask) | (value << bitsUsed));
      }
      else
      {
        d[p] = ((d[p] & (Maxvalue >> bitsFree)) | (value << bitsUsed));
        d[p + 1] = ((d[p + 1] & (Maxvalue << (bits - bitsFree))) | (value >> bitsFree));
      }

      Offset += bits;
    }

    ulong Read(int bits)
    {
      if (bits <= 0)
      {
        return 0;
      }

      int p = Offset >> Indexshift;
      int bitsUsed = Offset & Usedmask;
      ulong first = _buffer[p] >> bitsUsed;
      int remainingBits = bits - (Bitcount - bitsUsed);

      ulong value;

      if (remainingBits < 1)
      {
        value = (first & (Maxvalue >> (Bitcount - bits)));
      }
      else
      {
        ulong second = _buffer[p + 1] & (Maxvalue >> (Bitcount - remainingBits));
        value = (first | (second << (bits - remainingBits)));
      }

      Offset += bits;
      return value;
    }
  }
}