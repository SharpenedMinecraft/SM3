namespace Frontend
{
    public sealed class SimpleLinearEntityManager : IEntityManager
    {
        private int _nextId = 0;
        
        public IEntityId ReserveEntityId()
            => new SimpleEntityId(_nextId++);

        private readonly struct SimpleEntityId : IEntityId
        {
            public int Value { get; }

            public SimpleEntityId(int value)
            {
                Value = value;
            }

            public void Dispose()
            { }
        }
    }
}