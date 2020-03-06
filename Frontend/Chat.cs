using System.Text.Json.Serialization;

namespace SM3.Frontend
{
    public sealed class Chat
    {
        [JsonPropertyName("text")] public string? Text { get; set; }
        [JsonPropertyName("bold")] public bool? Bold { get; set; }
        [JsonPropertyName("italic")] public bool? Italic { get; set; }
        [JsonPropertyName("underlined")] public bool? Underlined { get; set; }
        [JsonPropertyName("strikethrough")] public bool? Strikethrough { get; set; }
        [JsonPropertyName("obfuscated")] public bool? Obfuscated { get; set; }
        [JsonPropertyName("color")] public string? Color { get; set; }
        [JsonPropertyName("insertion")] public string? Insertion { get; set; }
        [JsonPropertyName("extra")] public Chat[]? Extra { get; set; }
    }
}