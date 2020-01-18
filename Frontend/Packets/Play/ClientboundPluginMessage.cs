using System;

namespace Frontend.Packets.Play
{
    public readonly struct ClientboundPluginMessage : IWriteablePacket
    {
        public int Id => 0x19;

        public readonly string Identifier;
        public readonly ReadOnlyMemory<byte> Data;

        public ClientboundPluginMessage(string identifier, ReadOnlyMemory<byte> data)
        {
            Identifier = identifier;
            Data = data;
        }

        public void Write(IPacketWriter writer)
        {
            writer.WriteString(Identifier);
            writer.WriteBytes(Data.Span);
        }
    }
}