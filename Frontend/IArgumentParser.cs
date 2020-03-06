namespace SM3.Frontend
{
    public interface IArgumentParser
    {
        string Id { get; }

        string? SuggestionType { get; }
        
        void SerializeProperties(IPacketWriter writer);
    }
}