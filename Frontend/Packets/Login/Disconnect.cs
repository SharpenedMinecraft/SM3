using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Frontend.Packets.Login
{
    public struct Disconnect : IWriteablePacket
    {
        private static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            IgnoreNullValues = true
        };
        
        public readonly int Id => 0x00;
        public readonly bool IsServerbound => false;
        public readonly MCConnectionStage Stage => MCConnectionStage.Login;

        public int CalculateSize()
        {
            var length = JsonSerializer.SerializeToUtf8Bytes(Message, _jsonSerializerOptions).Length;

            return length + MCPacketWriter.GetVarIntSize(length);
        }

        public Disconnect(Chat message)
        {
            Message = message;
        }

        public Chat Message;
        
        public readonly void Write(IPacketWriter writer)
        {
            var data = JsonSerializer.SerializeToUtf8Bytes(Message, _jsonSerializerOptions);
            
            writer.WriteVarInt(data.Length);
            writer.WriteBytes(data);
        }
    }
}