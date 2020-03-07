using System.Diagnostics.CodeAnalysis;
using App.Metrics;
using App.Metrics.Formatters.Json;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SM3.Network;

namespace SM3.Frontend
{
    [SuppressMessage("ReSharper", "CA1801")]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IPacketReaderFactory, MCPacketReaderFactory>();
            services.AddSingleton<IPacketWriterFactory, MCPacketWriterFactory>();
            services.AddSingleton<IPacketQueueFactory, MCPacketQueueFactory>();
            services.AddSingleton<IPacketResolver, MCPacketResolver>();
            services.AddSingleton<IPacketHandler, MCPacketHandler>();
            services.AddSingleton<ITagProvider, FileTagProvider>();
            services.AddSingleton<ICommandProvider, CommandProvider>();
            services.AddSingleton<ITeleportManager, TeleportManager>();
            services.AddSingleton<IBroadcastQueue, BroadcastQueue>();
            services.AddSingleton<IDimensionResolver, MCDimensionResolver>(
                provider => new MCDimensionResolver(new IDimension[]
                {
                    new Overworld(),
                }, provider.GetRequiredService<IMetrics>()));
            services.AddMinecraftServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMetricsEndpoint(new MetricsPrometheusTextOutputFormatter());
            app.UseMetricsTextEndpoint(new MetricsJsonOutputFormatter());
            app.UseEnvInfoEndpoint();
            /*
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });*/
        }
    }
}
