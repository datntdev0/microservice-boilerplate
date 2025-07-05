using datntdev.Microservice.Identity.Web.Host.Components;

namespace datntdev.Microservice.Identity.Web.Host;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.
        builder.Services.AddRazorComponents();

        // Register mideware as transient instances
        builder.Services.AddTransient<Middlewares.AppSettingCookieMiddleware>();

        // Register scoped services
        builder.Services.AddScoped<Services.AppSettingService>();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseAntiforgery();
        app.UseMiddleware<Middlewares.AppSettingCookieMiddleware>();

        app.MapStaticAssets();
        app.MapRazorComponents<App>();

        app.Run();
    }
}
