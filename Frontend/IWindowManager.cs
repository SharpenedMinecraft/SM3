namespace Frontend
{
    public interface IWindowManager
    {
        IWindow OpenWindow { get; }
        T Open<T>() where T : IWindow;
        void Close(IWindow window, bool clientInitiated);
    }
}