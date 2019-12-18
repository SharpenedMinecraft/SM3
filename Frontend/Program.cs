using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Frontend
{
    public class Program
    {
        public static Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();
            Console.WriteLine($"Log of SM3 @ {DateTime.UtcNow}");
            Console.WriteLine($"Version: 0.1.0");
            return host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureLogging(((context, builder)
                                              => builder.AddConsole((options => options.IncludeScopes = true))
                                                        .AddDebug()
                                                        .AddEventSourceLogger()))
                        .UseKestrel(((context, options) =>
                        {
                            options.ListenAnyIP(25565, listenOptions =>
                            {
                                listenOptions.Protocols = HttpProtocols.None;
                                listenOptions.UseConnectionHandler<MCConnectionHandler>();
                            });
                        }))
                        .UseStartup<Startup>();
                });
    }
}
