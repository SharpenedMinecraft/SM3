using Microsoft.Extensions.DependencyInjection;

namespace SM3
{
    public static class ServiceExtensions
    {
        public static void AddMinecraftServices(this IServiceCollection services)
        {
            services.AddSingleton<IRandomProvider, JavaRandomProvider>();
            services.AddSingleton<IEntityRegistry, FileEntityRegistry>();
        }
    }
}