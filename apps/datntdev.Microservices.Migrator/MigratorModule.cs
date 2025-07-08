using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Identity.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityApplicationDbContext = datntdev.Microservices.Identity.Application.IdentityApplicationDbContext;

namespace datntdev.Microservices.Migrator
{
    [DependOn(typeof(IdentityApplicationModule))]
    internal class MigratorModule : BaseModule
    {
        public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
        {
            ConfigureDbContextSqlServer(services, configs);
        }

        private void ConfigureDbContextSqlServer(IServiceCollection services, IConfigurationRoot configs)
        {
            var migrationsAssembly = GetType().Assembly.GetName().Name;
            services.ConfigureDbContext<IdentityApplicationDbContext>(
                opt => opt.UseSqlServer(configs.GetConnectionString("IdentityService"),
                    o => o. MigrationsAssembly(migrationsAssembly)));
        }
    }
}
