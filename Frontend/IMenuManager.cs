namespace Frontend
{
    public interface IMenuManager
    {
        IMenu? OpenWindow { get; }
        T Open<T>() where T : IMenu;
        void Close(IMenu menu, bool clientInitiated);
    }
}