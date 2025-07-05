using datntdev.Microservices.Identity.Web.Host;
using datntdev.Microservices.Identity.Web.Host.Components;
using datntdev.Microservices.Identity.Web.Host.Middlewares;
using datntdev.Microservices.ServiceDefaults.Hosting;

ServiceBootstrapBuilder.CreateWebApplication<Startup>(args).Run();

internal class Startup(IWebHostEnvironment env) : ServiceStartup(env)
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddDefaultOpenTelemetry(_hostingEnvironment, _hostingConfiguration);
        services.AddServiceBootstrap<IdentityWebHostModule>(_hostingConfiguration);
        services.AddDefaultServiceDiscovery();

        // Add services to the container.
        services.AddRazorComponents();
        services.AddControllers();

        // Add Authentication and Authorization services
        services.AddCascadingAuthenticationState();
        services.AddAuthentication().AddCookie(x => x.LoginPath = "/auth/signin");
        services.AddAuthorization().AddAuthorizationCore();

        // Add Middlewares as Transient Instances
        services.AddTransient<AppSettingCookieMiddleware>();
    }

    public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseServiceBootstrap<IdentityWebHostModule>(_hostingConfiguration);

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseAntiforgery();

        app.UseEndpoints(configure =>
        {
            configure.MapControllers();
            configure.MapRazorComponents<App>();
            configure.MapStaticAssets();
        });

        app.UseMiddleware<AppSettingCookieMiddleware>();
    }
}

