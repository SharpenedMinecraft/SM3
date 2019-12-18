using System.Text.Json.Serialization;

namespace Frontend
{
    internal sealed class StatusResponse
    {
        [JsonPropertyName("version")] public VersionPayload Version { get; set; }
        [JsonPropertyName("players")] public PlayersPayload Players { get; set; }
        [JsonPropertyName("description")] public Chat Description { get; set; }
        [JsonPropertyName("favicon")] public string? FavIcon { get; set; }

        public StatusResponse(VersionPayload version, PlayersPayload players, Chat description, string? favIcon)
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