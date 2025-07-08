using datntdev.Microservices.Admin.Web.Host;
using datntdev.Microservices.ServiceDefaults.Hosting;
using OpenIddict.Validation.AspNetCore;
using Scalar.AspNetCore;

ServiceBootstrapBuilder.CreateWebApplication<Startup>(args).Run();

internal class Startup(IWebHostEnvironment env) : WebServiceStartup(env)
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddDefaultOpenTelemetry(_hostingEnvironment, _hostingConfiguration);
        services.AddServiceBootstrap<AdminWebHostModule>(_hostingConfiguration);
        services.AddDefaultServiceDiscovery();

        var defaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        services.AddAuthentication(o => o.DefaultScheme = defaultScheme);
        services.AddAuthorization().AddAuthorizationCore();

        // Add services to the container.
        services.AddControllers();
        services.AddOpenApi();
    }

    public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseServiceBootstrap<AdminWebHostModule>(_hostingConfiguration);

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(configure =>
        {
            configure.MapControllers();
            configure.MapOpenApi();
            configure.MapScalarApiReference();
        });
    }
}
