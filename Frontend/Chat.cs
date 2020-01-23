using System.ServiceModel.Channels;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Frontend
{
    public sealed class Chat : IWriteableSpecialType
    {
        private static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            IgnoreNullValues = true
        };
        
        [JsonPropertyName("text")] public string? Text { get; set; }
        [JsonPropertyName("bold")] public bool? Bold { get; set; }
        [JsonPropertyName("italic")] public bool? Italic { get; set; }
        [JsonPropertyName("underlined")] public bool? Underlined { get; set; }
        [JsonPropertyName("strikethrough")] public bool? Strikethrough { get; set; }
        [JsonPropertyName("obfuscated")] public bool? Obfuscated { get; set; }
        [JsonPropertyName("color")] public string? Color { get; set; }
        [JsonPropertyName("insertion")] public string? Insertion { get; set; }
        [JsonPropertyName("extra")] public Chat[]? Extra { get; set; }
        
        public void Write(IPacketWriter writer)
        {
            var data = JsonSerializer.SerializeToUtf8Bytes(this, _jsonSerializerOptions);
            
            writer.WriteVarInt(data.Length);
            writer.WriteBytes(data);
        }
    }
}