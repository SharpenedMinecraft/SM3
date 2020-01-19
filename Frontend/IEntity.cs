namespace Frontend
{
    public interface IEntity
    {
        int Id { get; set; }
        void Process(IEntityManager preTick, IEntityManager postTick);
    }
}