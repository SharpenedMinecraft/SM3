using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SM3.Frontend
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location));
            using var host = CreateHostBuilder(args).Build();
            Console.WriteLine($"Log of SM3 @ {DateTime.UtcNow}");
            Console.WriteLine("Version: 0.5.1");
            Console.WriteLine($"Compatible with Protocol {Constants.ProtocolVersion} Display Version {Constants.VersionName}");
            #if NO_OPTIMIZATION
            Console.WriteLine("This Build uses no special Optimizations! Might lead to heavy loss of performance!");
            #endif
            #if DUMP_WRITE_BYTES
            Console.WriteLine("This Build will dump any byte*s* written to the Console. I've warned you");
            #endif
            
            // load tags
            host.Services.GetRequiredService<ITagProvider>().Load();
            
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
                        .UseKestrel((context, options) =>
                        {
                            options.AllowSynchronousIO = true; // Normally false, but the Prometheus Provider of App Metrics requires this.
                            options.ListenAnyIP(25565, listenOptions =>
                            {
                                listenOptions.Protocols = HttpProtocols.None;
                                listenOptions.UseConnectionHandler<MCConnectionHandler>();
                            });
                            options.ListenAnyIP(80);
                        });
                });
    }
}
