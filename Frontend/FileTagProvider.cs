using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Frontend
{
    public sealed class FileTagProvider : ITagProvider
    {
        private const string TagDir = "./Data/tags/";
        private const string BlockDir = TagDir + "blocks/";
        private const string EntityDir = TagDir + "entity_types/";
        private const string FluidDir = TagDir + "fluids/";
        private const string ItemDir = TagDir + "items/";
        
        private Dictionary<string, Tag> _fluidTags = new Dictionary<string, Tag>();
        private Dictionary<string, Tag> _blockTags = new Dictionary<string, Tag>();
        private Dictionary<string, Tag> _entityTypes = new Dictionary<string, Tag>();
        private Dictionary<string, Tag> _itemTags = new Dictionary<string, Tag>();

        public IReadOnlyDictionary<string, Tag> FluidTags => _fluidTags;
        public IReadOnlyDictionary<string, Tag> BlockTags => _blockTags;
        public IReadOnlyDictionary<string, Tag> EntityTypes => _entityTypes;
        public IReadOnlyDictionary<string, Tag> ItemTags => _itemTags;

        private readonly ILogger _logger;

        public FileTagProvider(ILogger<FileTagProvider> logger)
        {
            _logger = logger;
        }

        public void Load()
        {
            _logger.LogInformation($"Loading Tag Files from {Path.GetFullPath(TagDir)}");
            ParseBlocks();
            _logger.LogInformation($"Loaded {_blockTags.Count} Block Tags");
            ParseEntityTypes();
            _logger.LogInformation($"Loaded {_entityTypes.Count} Entity Types");
            ParseFluids();
            _logger.LogInformation($"Loaded {_fluidTags.Count} Fluid Tags");
            ParseItems();
            _logger.LogInformation($"Loaded {_itemTags.Count} Item Tags Tags");
        }

        private void ParseBlocks()
        {
            foreach(var fileName in Directory.EnumerateFiles(BlockDir, "*.json"))
            {
                using var _ = _logger.BeginScope(fileName);
                try
                {
                    var jsonReader = new Utf8JsonReader(File.ReadAllBytes(fileName));
                    do
                    {
                        jsonReader.Read();

                        if (!TryParseTagInfo(ref jsonReader, out var replace, out var values) ||
                            (_blockTags.ContainsKey(fileName) && !(replace ?? false)))
                        {
                            continue;
                        }

                        var identifier = Path.GetFileNameWithoutExtension(fileName);
                        _blockTags[identifier] = new Tag(identifier, ResolveBlockNames(values));
                    } while (jsonReader.TokenType != JsonTokenType.None &&
                             jsonReader.TokenType != JsonTokenType.EndObject);
                }
                catch (Exception e)
                {
                    _logger.LogCritical(e, $"Failed to load {fileName}");
                    throw;
                }
            }
        }

        private void ParseEntityTypes()
        {
            foreach(var fileName in Directory.EnumerateFiles(EntityDir, "*.json"))
            {
                var jsonReader = new Utf8JsonReader(File.ReadAllBytes(fileName));
                do
                {
                    jsonReader.Read();
                    
                    if (!TryParseTagInfo(ref jsonReader, out var replace, out var values) ||
                        (_entityTypes.ContainsKey(fileName) && !(replace ?? false)))
                    {
                        continue;
                    }
                    
                    var identifier = Path.GetFileNameWithoutExtension(fileName);
                    _entityTypes[identifier] = new Tag(identifier, ResolveEntityNames(values));
                } while (jsonReader.TokenType != JsonTokenType.None && jsonReader.TokenType != JsonTokenType.EndObject);
            }
        }
        
        private void ParseFluids()
        {
            foreach(var fileName in Directory.EnumerateFiles(FluidDir, "*.json"))
            {
                var jsonReader = new Utf8JsonReader(File.ReadAllBytes(fileName));
                do
                {
                    jsonReader.Read();

                    if (!TryParseTagInfo(ref jsonReader, out var replace, out var values) ||
                        (_fluidTags.ContainsKey(fileName) && !(replace ?? false)))
                    {
                        continue;
                    }

                    var identifier = Path.GetFileNameWithoutExtension(fileName);
                    _fluidTags[identifier] = new Tag(identifier, ResolveBlockNames(values));
                } while (jsonReader.TokenType != JsonTokenType.None && jsonReader.TokenType != JsonTokenType.EndObject);
            }
        }

        private void ParseItems()
        {
            foreach(var fileName in Directory.EnumerateFiles(ItemDir, "*.json"))
            {
                var jsonReader = new Utf8JsonReader(File.ReadAllBytes(fileName));
                do
                {
                    jsonReader.Read();
                    
                    if (!TryParseTagInfo(ref jsonReader, out var replace, out var values) ||
                        (_itemTags.ContainsKey(fileName) && !(replace ?? false)))
                    {
                        continue;
                    }
                        
                    var identifier = Path.GetFileNameWithoutExtension(fileName);
                    _itemTags[identifier] = new Tag(identifier, ResolveItemNames(values));
                } while (jsonReader.TokenType != JsonTokenType.None && jsonReader.TokenType != JsonTokenType.EndObject);
            }
        }
        
        private int[] ResolveItemNames(List<string> values)
        {
            var ids = new int[values.Count];
            for (var i = 0; i < values.Count; i++)
            {
                // TODO: Resolve item Names
                ids[i] = 0;
            }

            return ids;
        }
        
        private int[] ResolveEntityNames(List<string> values)
        {
            var ids = new int[values.Count];
            for (var i = 0; i < values.Count; i++)
            {
                // TODO: Resolve entity Names
                ids[i] = 0;
            }

            return ids;
        }

        private int[] ResolveBlockNames(List<string> values)
        {
            var ids = new int[values.Count];
            for (var i = 0; i < values.Count; i++)
            {
                // TODO: Resolve block Names
                ids[i] = 0;
            }

            return ids;
        }

        private bool TryParseTagInfo(ref Utf8JsonReader jsonReader, [NotNullWhen(true)] out bool? replace, [NotNullWhen(true)] out List<string>? values)
        {
            bool b = false;
            replace = null;
            values = null;
            
            while (jsonReader.TokenType == JsonTokenType.PropertyName)
            {
                var name = jsonReader.GetString();
                jsonReader.Read();
                if (name == "replace")
                    replace = jsonReader.GetBoolean();
                if (name == "values")
                {
                    values = ParseValues(ref jsonReader);
                }

                jsonReader.Read();

                b = true;
            }

            return b;
        }

        private List<string> ParseValues(ref Utf8JsonReader jsonReader)
        {
            var values = new List<string>();
            Debug.Assert(jsonReader.TokenType == JsonTokenType.StartArray);
            jsonReader.Read();
            while (jsonReader.TokenType != JsonTokenType.EndArray)
            {
                values.Add(jsonReader.GetString());
                jsonReader.Read();
            }

            return values;
        }
    }
}