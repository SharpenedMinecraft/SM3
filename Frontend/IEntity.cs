namespace Frontend
{
    public interface IEntity
    {
        IEntityId Id { get; }
        int DimensionId { get; }
    }
}