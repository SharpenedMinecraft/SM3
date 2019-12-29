using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Frontend.Packets.Login
{
    public readonly struct Disconnect : IWriteablePacket
    {
        private static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            IgnoreNullValues = true
        };
        
        public int Id => 0x00;
        public MCConnectionStage Stage => MCConnectionStage.Login;

        public int CalculateSize()
        {
            var length = JsonSerializer.SerializeToUtf8Bytes(Message, _jsonSerializerOptions).Length;

            return length + MCPacketWriter.GetVarIntSize(length);
        }

        public Disconnect(Chat message)
        {
            Message = message;
        }

        public readonly Chat Message;
        
        public readonly void Write(IPacketWriter writer)
        {
            var data = JsonSerializer.SerializeToUtf8Bytes(Message, _jsonSerializerOptions);
            
            writer.WriteVarInt(data.Length);
            writer.WriteBytes(data);
        }
    }
}