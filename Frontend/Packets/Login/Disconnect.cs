using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Frontend.Packets.Login
{
    public struct Disconnect : IPacket
    {
        private static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            IgnoreNullValues = true
        };
        
        public readonly int Id => 0x00;
        public readonly bool IsServerbound => false;
        public readonly MCConnectionStage Stage => MCConnectionStage.Login;

        public readonly int Size
        {
            get
            {
                var length = JsonSerializer.SerializeToUtf8Bytes(Message, _jsonSerializerOptions).Length;

                return length + MCPacketWriter.GetVarIntSize(length);
            }   
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

        public void Read(IPacketReader reader)
        {
            var length = reader.ReadVarInt();
            var data = reader.ReadBytes(length);

            Message = JsonSerializer.Deserialize<Chat>(data, _jsonSerializerOptions);
        }

        public readonly void Process(ILogger logger, IConnectionState connectionState, IPacketQueue packetQueue)
        {  }
    }
}