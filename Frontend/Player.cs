namespace Frontend
{
    public class Player : IEntity
    {
        public Player(IEntityId id, int dimensionId)
        {
            Id = id;
            DimensionId = dimensionId;
        }

        public IEntityId Id { get; }
        
        public int DimensionId { get; }
        
        public string Username { get; set; }
    }
}