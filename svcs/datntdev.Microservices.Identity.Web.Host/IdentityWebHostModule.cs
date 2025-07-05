using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Identity.Application;
using datntdev.Microservices.Identity.Application.Authorization.Roles;
using datntdev.Microservices.Identity.Application.Authorization.Users;
using datntdev.Microservices.Identity.Application.Repositories.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservices.Identity.Web.Host
{
    [DependOn(typeof(IdentityApplicationModule))]
    public class IdentityWebHostModule : BaseModule
    {
        public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
        {
            // Register Entity Framework Core with OpenIddict support
            AddDatabaseContext(services, configs);

            // Register OpenIddict
            AddIdentityOpenIddict(services);

            // Register Application services
            services.AddScoped<Services.AppSettingService>();
        }

        private void AddDatabaseContext(IServiceCollection services, IConfigurationRoot configs)
        {
            var connectionString = configs.GetConnectionString("DefaultConnection");
            var migrationsAssembly = GetType().Assembly.GetName().Name;
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseSqlServer(connectionString, o => o.MigrationsAssembly(migrationsAssembly));
                opt.UseOpenIddict(); // Register OpenIddict with the DbContext
            });
        }

        private static void AddIdentityOpenIddict(IServiceCollection services)
        {
            services.AddIdentity<AppUserEntity, AppRoleEntity>()
                .AddSignInManager()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                        .UseDbContext<ApplicationDbContext>();
                })
                .AddServer(options =>
                {
                    options.RequireProofKeyForCodeExchange()
                        .AllowAuthorizationCodeFlow()
                        .SetTokenEndpointUris("/connect/token")
                        .SetAuthorizationEndpointUris("/connect/authorize");

                    // TODO: Disable access token encryption for debug
                    // edit this code for applying multi higher environments
                    options.DisableAccessTokenEncryption()
                        .AddEphemeralEncryptionKey()
                        .AddEphemeralSigningKey();
                    
                    options.UseAspNetCore()
                        .EnableTokenEndpointPassthrough()
                        .EnableAuthorizationEndpointPassthrough(); ;
                });
        }
    }
}
