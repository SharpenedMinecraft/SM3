using System;
using Frontend.Windows;

namespace Frontend
{
    public sealed class WindowManagerFactory : IWindowManagerFactory
    {
        private readonly IServiceProvider _provider;

        public WindowManagerFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IWindowManager CreateManager(IPacketQueue clientQueue) => new WindowManager(_provider, clientQueue, new PlayerInventory());
    }
}
