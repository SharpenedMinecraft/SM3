namespace Frontend
{
    public interface IMenuManagerFactory
    {
        IMenuManager CreateManager(IPacketQueue clientQueue);
    }
}