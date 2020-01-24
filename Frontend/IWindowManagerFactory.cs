namespace Frontend
{
    public interface IWindowManagerFactory
    {
        IWindowManager CreateManager(IPacketQueue clientQueue);
    }
}