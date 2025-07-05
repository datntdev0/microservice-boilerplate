using datntdev.Microservices.Common.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.ServiceDefaults.Hosting
{
    public abstract class ServiceStartup(IWebHostEnvironment env)
    {
        protected readonly IWebHostEnvironment _hostingEnvironment = env;
        protected readonly IConfigurationRoot _hostingConfiguration = AppConfiguration.Get(env);

        public abstract void ConfigureServices(IServiceCollection services);
        public abstract void Configure(IApplicationBuilder app, IWebHostEnvironment env);
    }
}
