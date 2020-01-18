using System.Text.Json.Serialization;

namespace Frontend
{
    public readonly struct Tag
    {
        public string Id { get; }
        public int[] Values { get; }

        public Tag(string id, int[] values)
        {
            Id = id;
            Values = values;
        }
    }
}