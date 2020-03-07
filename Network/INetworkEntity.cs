namespace SM3.Network
{
    public interface INetworkEntity : IEntity
    {
        void WriteMetadata(EntityMetadata metadata);
    }
}