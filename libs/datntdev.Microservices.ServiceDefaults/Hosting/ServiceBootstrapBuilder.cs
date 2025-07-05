using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace datntdev.Microservices.ServiceDefaults.Hosting
{
    public static class ServiceBootstrapBuilder
    {
        public static IHost CreateDefaultHost<TStartup>(string[] args) where TStartup : ServiceStartup
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TStartup>();
                })
                .Build();
        }

        public static IHost CreateWebApplication<TStartup>(string[] args) where TStartup : ServiceStartup
        {
            var builder = WebApplication.CreateBuilder(args);
            var startup = Activator.CreateInstance(typeof(TStartup), builder.Environment);
            ((TStartup)startup!).ConfigureServices(builder.Services);
            var app = builder.Build();
            ((TStartup)startup).Configure(app, builder.Environment);
            return app;
        }
    }
}
