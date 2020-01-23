using System;
using Microsoft.Extensions.DependencyInjection;

namespace Frontend
{
    public sealed class MenuManager : IMenuManager
    {
        private readonly IServiceProvider _provider;
        private readonly IPacketQueue _queue;
        private byte _id = 1;

        public IMenu? OpenWindow { get; private set; }

        public MenuManager(IServiceProvider provider, IPacketQueue queue)
        {
            _provider = provider;
            _queue = queue;
        }
        
        public T Open<T>()
            where T : IMenu
        {
            if (OpenWindow != null)
            {
                Close(OpenWindow, false);
                OpenWindow = null;
            }
            
            var instance = ActivatorUtilities.CreateInstance<T>(_provider);

            do
            {
                instance.Id = unchecked(_id++);
            } while (instance.Id == 0);

            foreach (var packet in instance.OpenPackets) _queue.Write(packet);

            return instance;
        }

        public void Close(IMenu menu, bool clientInitiated)
        {
            if (!clientInitiated)
                foreach (var packet in menu.ClosePackets) _queue.Write(packet);

            OpenWindow = null;
        }
    }
}