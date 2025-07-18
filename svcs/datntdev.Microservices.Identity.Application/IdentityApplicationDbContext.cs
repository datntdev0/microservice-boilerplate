using datntdev.Microservices.Identity.Application.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservices.Identity.Application
{
    public class IdentityApplicationDbContext(DbContextOptions<IdentityApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<AppTenantEntity> AppTenants { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AppTenantEntity>();
        }
    }
}
