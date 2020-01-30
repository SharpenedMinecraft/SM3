using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Frontend.Packets.Play
{
    public readonly struct ClientboundChatMessage : IWriteablePacket
    {
        private static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            IgnoreNullValues = true
        };
        public int Id => 0x0F;

        public readonly Chat Chat;
        public readonly byte Position;

        public ClientboundChatMessage(Chat chat, byte position)
        {
            Chat = chat;
            Position = position;
        }

        public void Write(IPacketWriter writer)
        {
            var data = JsonSerializer.SerializeToUtf8Bytes(Chat, _jsonSerializerOptions);

            writer.WriteVarInt(data.Length);
            writer.WriteBytes(data);
            writer.WriteUInt8(Position);
        }

    }
}
