using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Identity.Application;
using datntdev.Microservices.Identity.Contracts;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservices.Identity.Web.Host
{
    [DependOn(typeof(IdentityApplicationModule))]
    public class IdentityWebHostModule : BaseModule
    {
        public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
        {
            // Register Entity Framework Core with OpenIddict support
            ConfigureDbContextSqlServer(services, configs);
            ConfigureAuthentication(services, configs);

            // Register Application services
            services.AddScoped<Services.AppSettingService>();
        }

        private static void ConfigureDbContextSqlServer(IServiceCollection services, IConfigurationRoot configs)
        {
            var connectionString = configs.GetConnectionString("DefaultConnection");
            services.ConfigureDbContext<IdentityApplicationDbContext>(
                opt => opt.UseSqlServer(connectionString));
        }

        private static void ConfigureAuthentication(IServiceCollection services, IConfigurationRoot configs)
        {
            services.AddAuthentication(Constants.AuthenticationScheme)
                .AddCookie(Constants.AuthenticationScheme, opt =>
                {
                    opt.LoginPath = Constants.SignInPath;
                    opt.LogoutPath = Constants.SignOutPath;
                });

            services.AddAuthorization().AddAuthorizationCore();
        }
    }
}
