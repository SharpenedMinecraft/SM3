using System;

namespace Frontend
{
    public sealed class MenuManagerFactory : IMenuManagerFactory
    {
        private readonly IServiceProvider _provider;

        public MenuManagerFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IMenuManager CreateManager(IPacketQueue clientQueue) => new MenuManager(_provider, clientQueue);
    }
}