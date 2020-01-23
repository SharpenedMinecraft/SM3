namespace Frontend
{
    public interface IMenuManager
    {
        IMenu? OpenMenu { get; }
        T Open<T>() where T : IMenu;
        void Close(IMenu menu, bool clientInitiated);
    }
}