using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Identity.Application;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservices.Identity.Web.Host
{
    [DependOn(typeof(IdentityApplicationModule))]
    public class IdentityWebHostModule : BaseModule
    {
        public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
        {
            // Register Entity Framework Core with OpenIddict support
            ConfigureqDbContextSqlServer(services, configs);

            // Register Application services
            services.AddScoped<Services.AppSettingService>();
        }

        private static void ConfigureqDbContextSqlServer(IServiceCollection services, IConfigurationRoot configs)
        {
            var connectionString = configs.GetConnectionString("DefaultConnection");
            services.ConfigureDbContext<IdentityApplicationDbContext>(
                opt => opt.UseSqlServer(connectionString));
        }
    }
}
