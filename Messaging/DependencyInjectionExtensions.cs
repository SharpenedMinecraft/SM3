using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.System.Text.Json;

namespace Messaging
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection collection) =>
            collection.AddStackExchangeRedisExtensions<SystemTextJsonSerializer>(new RedisConfiguration
            {
                Hosts = new[]
                {
                    new RedisHost
                    {
                        Host = "localhost",
                        Port = 6379
                    }
                }
            });
    }
}