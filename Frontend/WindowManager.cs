using System;
using Microsoft.Extensions.DependencyInjection;

namespace Frontend
{
    public sealed class WindowManager : IWindowManager
    {
        private readonly IServiceProvider _provider;
        private readonly IPacketQueue _queue;
        private readonly IWindow? _defaultWindow;
        private byte _id = 1;

        public IWindow? OpenWindow { get; private set; }

        public WindowManager(IServiceProvider provider, IPacketQueue queue, IWindow? defaultWindow)
        {
            _provider = provider;
            _queue = queue;
            _defaultWindow = defaultWindow;
            OpenWindow = _defaultWindow;
        }

        public T Open<T>()
            where T : IWindow
        {
            if (OpenWindow != null)
            {
                Close(OpenWindow, false);
            }

            var instance = ActivatorUtilities.CreateInstance<T>(_provider);

            do
            {
                instance.Id = unchecked((sbyte)(_id++));
            } while (instance.Id == 0);

            foreach (var packet in instance.OpenPackets) _queue.Write(packet);

            OpenWindow = instance;
            return instance;
        }

        public void Close(IWindow window, bool clientInitiated)
        {
            if (!clientInitiated)
                foreach (var packet in window.ClosePackets) _queue.Write(packet);

            OpenWindow = _defaultWindow;
        }
    }
}
