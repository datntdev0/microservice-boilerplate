using datntdev.Microservices.Common.Modular;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ServiceDiscovery;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace datntdev.Microservices.ServiceDefaults.Hosting
{
    public static class ServiceBootstrapExtensions
    {
        public static IServiceCollection AddServiceBootstrap<TStartupModule>(this IServiceCollection services, IConfigurationRoot configs) 
            where TStartupModule : BaseModule
        {
            var bootstrapper = new ServiceBootstrap<TStartupModule>();
            bootstrapper.ConfigureServices(services, configs);
            return services.AddSingleton(bootstrapper);
        }

        public static IApplicationBuilder UseServiceBootstrap<TStartupModule>(this IApplicationBuilder app, IConfigurationRoot configs)
            where TStartupModule : BaseModule
        {
            var bootstrapper = app.ApplicationServices.GetRequiredService<ServiceBootstrap<TStartupModule>>();
            bootstrapper.Configure(app.ApplicationServices, configs);
            return app;
        }

        public static IServiceCollection AddDefaultOpenTelemetry(
            this IServiceCollection services, IWebHostEnvironment env, IConfigurationRoot config)
        {
            services.AddLogging(builder =>
            {
                builder.AddOpenTelemetry(config =>
                {
                    config.IncludeFormattedMessage = true;
                    config.IncludeScopes = true;
                });
            });

            services.AddOpenTelemetry()
                .WithMetrics(metrics =>
                {
                    metrics.AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddRuntimeInstrumentation();
                })
                .WithTracing(tracing =>
                {
                    tracing.AddSource(env.ApplicationName)
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation();
                });

            services.AddOpenTelemetry().UseOtlpExporter();

            return services;
        }

        public static IServiceCollection AddDefaultServiceDiscovery(this IServiceCollection services)
        {
            services.AddServiceDiscovery();

            services.ConfigureHttpClientDefaults(http =>
            {
                // Turn on resilience by default
                http.AddStandardResilienceHandler();

                // Turn on service discovery by default
                http.AddServiceDiscovery();
            });

            services.Configure<ServiceDiscoveryOptions>(options =>
            {
                options.AllowedSchemes = ["https"];
            });

            return services;
        }
    }
}
