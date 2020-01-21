﻿using System.Collections.Generic;
 using System.IO;
 using System.Text.Json;

 namespace Frontend
{
    public sealed class FileEntityRegistry : IEntityRegistry
    {
        public FileEntityRegistry()
        {
            const string registryPath = "./Data/registries.json";
            var reader = new Utf8JsonReader(File.ReadAllBytes(registryPath));

            while (true)
            {
                reader.Read();
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    if (reader.ValueTextEquals("minecraft:entity_type"))
                        break;
                    reader.Skip();
                }
            }
            
            reader.Read(); // Start Object

            while (reader.TokenType != JsonTokenType.EndObject)
            {
                reader.Read();
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = reader.GetString();
                    if (propertyName == "protocol_id")
                    {
                        reader.Read();
                    }
                    else if (propertyName == "default")
                    {
                        reader.Read();
                        Default = reader.GetString();
                    }
                    else if (propertyName == "entries")
                    {
                        reader.Read(); // Start Object 1
                        while (true)
                        {
                            reader.Read();

                            if (reader.TokenType == JsonTokenType.EndObject) // End Object 1
                                break;

                            var key = reader.GetString();
                            reader.Read(); // Start Object 2
                            reader.Read(); // "protocol_id"
                            reader.Read();
                            var value = reader.GetInt32();
                            _entities[key] = value;
                            reader.Read(); // End Object 2
                        }
                    }
                }
            }
        }

        public string Default { get; }
        private Dictionary<string, int> _entities = new Dictionary<string, int>();
        public int this[string id] => _entities[id];
    }
}