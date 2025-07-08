using datntdev.Microservices.Migrator;
using datntdev.Microservices.ServiceDefaults.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

ServiceBootstrapBuilder.CreateHostApplication<Startup>(args).Run();

internal class Startup(IHostEnvironment env) : AppServiceStartup(env)
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddServiceBootstrap<MigratorModule>(_hostingConfiguration);
        services.AddHostedService<MigratorHostedWorker>();
    }
}