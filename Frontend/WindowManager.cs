using System;
using Microsoft.Extensions.DependencyInjection;

namespace Frontend
{
    public sealed class WindowManager : IWindowManager
    {
        private readonly IServiceProvider _provider;
        private readonly IPacketQueue _queue;
        private byte _id = 0;

        public IWindow? OpenWindow { get; private set; }

        public WindowManager(IServiceProvider provider, IPacketQueue queue)
        {
            _provider = provider;
            _queue = queue;
        }
        
        public T Open<T>()
            where T : IWindow
        {
            if (OpenWindow != null)
            {
                Close(OpenWindow, false);
                OpenWindow = null;
            }
            
            var instance = ActivatorUtilities.CreateInstance<T>(_provider);
            instance.Id = unchecked(_id++);

            foreach (var packet in instance.OpenPackets) _queue.Write(packet);

            return instance;
        }

        public void Close(IWindow window, bool clientInitiated)
        {
            if (!clientInitiated)
                foreach (var packet in window.ClosePackets) _queue.Write(packet);

            OpenWindow = null;
        }
    }
}