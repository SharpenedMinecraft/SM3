namespace SM3
{
    public interface IArgumentParser
    {
        string Id { get; }

        string? SuggestionType { get; }
        
        // void SerializeProperties(IPacketWriter writer);
    }
}