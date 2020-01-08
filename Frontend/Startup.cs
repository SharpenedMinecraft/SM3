using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Extensions.Configuration;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Formatters.Json;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Frontend
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
            services.AddSingleton<IEntityManager, SimpleLinearEntityManager>();
            services.AddSingleton<ITagProvider, FileTagProvider>();
            services.AddSingleton<ICommandProvider, CommandProvider>();
            services.AddSingleton<ITeleportManager, TeleportManager>();
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
