using datntdev.Microservices.Identity.Application.Authorization.Roles;
using datntdev.Microservices.Identity.Application.Authorization.Users;
using datntdev.Microservices.Identity.Web.Host.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservices.Identity.Web.Host;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.
        builder.Services.AddRazorComponents();
        builder.Services.AddControllers();

        // Register Database Context
        AddDatabaseContext(builder);

        // Register mideware as transient instances
        builder.Services.AddTransient<Middlewares.AppSettingCookieMiddleware>();

        // Register Identity Core
        AddIdentityCore(builder);
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddAuthenticationCore();
        builder.Services.AddAuthorization().AddAuthorizationCore();

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
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAntiforgery();

        app.UseMiddleware<Middlewares.AppSettingCookieMiddleware>();

        app.MapStaticAssets();
        app.MapRazorComponents<App>();
        app.MapControllers();

        app.Run();
    }

    private static void AddDatabaseContext(WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        var migrationsAssembly = typeof(Program).Assembly.GetName().Name;
        builder.Services.AddDbContext<Application.Repositories.Data.ApplicationDbContext>(
            opt => opt.UseSqlServer(connectionString, o => o.MigrationsAssembly(migrationsAssembly)));
    }

    private static void AddIdentityCore(WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<AppUserEntity, AppRoleEntity>()
            .AddEntityFrameworkStores<Application.Repositories.Data.ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();
    }
}
