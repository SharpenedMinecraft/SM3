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
        public static void Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();
            Console.WriteLine($"Log of SM3 @ {DateTime.UtcNow}");
            Console.WriteLine($"Version: 0.1.0");
            #if AVX && !NO_OPTIMIZATION
            Console.WriteLine("This Build uses AVX if supported (DOWNCLOCKED OR NOT)");
            #endif
            #if NO_OPTIMIZATION
            Console.WriteLine("This Build uses no special Optimizations! Might lead to heavy loss of performance!")
            #endif
            #if DUMP_WRITE_BYTES
            Console.WriteLine("This Build will dump any byte*s* written to the Console. I've warned you");
            #endif
            host.Run();
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
                        .UseStartup<Startup>()
                        .UseKestrel(((context, options) =>
                        {
                            options.ListenAnyIP(25565, listenOptions =>
                            {
                                // listenOptions.Protocols = HttpProtocols.None;
                                listenOptions.UseConnectionHandler<MCConnectionHandler>();
                            });
                        }));
                });
    }
}
