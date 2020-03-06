using System.Text.Json;

namespace SM3.Frontend.Packets.Login
{
    // ReSharper disable once CA1815
    public readonly struct Disconnect : IWriteablePacket
    {
        private static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            IgnoreNullValues = true
        };
        
        public int Id => 0x00;

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