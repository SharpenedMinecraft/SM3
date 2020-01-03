using System;

namespace Frontend.Packets.Play
{
    public readonly struct ClientboundPluginMessage : IWriteablePacket
    {
        public int Id => 0x18;

        public readonly string Identifier;
        public readonly ReadOnlyMemory<byte> Data;

        public ClientboundPluginMessage(string identifier, ReadOnlyMemory<byte> data)
        {
            Identifier = identifier;
            Data = data;
        }

        public int CalculateSize()
            => MCPacketWriter.GetVarIntSize(Identifier.Length)
             + Identifier.Length
             + Data.Length;

        public void Write(IPacketWriter writer)
        {
            writer.WriteString(Identifier);
            writer.WriteBytes(Data.Span);
        }
    }
}