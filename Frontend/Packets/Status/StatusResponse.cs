using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace Frontend.Packets.Status
{
    public struct StatusResponse : IPacket
    {
        private static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            IgnoreNullValues = true
        };
        
        public readonly int Id => 0x00;
        public readonly MCConnectionStage Stage => MCConnectionStage.Status;
        public readonly bool IsServerbound => false;
        public int Size
        {
            get
            {
                if (_size == null)
                {
                    var length = JsonSerializer.SerializeToUtf8Bytes(Data, _jsonSerializerOptions).Length;
                    
                    _size = MCPacketWriter.GetVarIntSize(length)
                          + length;
                }

                return _size.Value;
            }
        }

        public Payload Data;
        
        private int? _size;
        
        public void Write(IPacketWriter writer)
        {
            var data = JsonSerializer.SerializeToUtf8Bytes(Data, _jsonSerializerOptions);
            writer.WriteVarInt(data.Length);
            writer.WriteBytes(data);
        }

        public void Read(IPacketReader reader)
        {
            Data = JsonSerializer.Deserialize<Payload>(reader.ReadString().ToString(), _jsonSerializerOptions);
        }

        public void Process(ILogger logger, IConnectionState connectionState, IPacketQueue packetQueue)
        {
            // nothing
        }
        
        public sealed class Payload
        {
            [JsonPropertyName("version")] public VersionPayload Version { get; set; }
            [JsonPropertyName("players")] public PlayersPayload Players { get; set; }
            [JsonPropertyName("description")] public Chat Description { get; set; }
            [JsonPropertyName("favicon")] public string? FavIcon { get; set; }

            public Payload(VersionPayload version, PlayersPayload players, Chat description, string? favIcon)
            {
                Version = version;
                Players = players;
                Description = description;
                FavIcon = favIcon;
            }

            public sealed class VersionPayload
            {
                [JsonPropertyName("name")] public string Name { get; set; }
                [JsonPropertyName("protocol")] public int Version { get; set; }

                public VersionPayload(string name, int version)
                {
                    Name = name;
                    Version = version;
                }
            }

            public sealed class PlayersPayload
            {
                [JsonPropertyName("max")] public int Max { get; set; }
                [JsonPropertyName("online")] public int Online { get; set; }
                [JsonPropertyName("sample")] public PlayersPayload.Player[]? Sample { get; set; }

                public PlayersPayload(int max, int online, Player[]? sample)
                {
                    Max = max;
                    Online = online;
                    Sample = sample;
                }

                public sealed class Player
                {
                    [JsonPropertyName("name")] public string Name { get; set; }
                    [JsonPropertyName("id")] public string Id { get; set; }

                    public Player(string name, string id)
                    {
                        Name = name;
                        Id = id;
                    }
                }
            }
        }
    }
}