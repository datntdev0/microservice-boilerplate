using datntdev.Microservices.Identity.Application.Authorization.Users.Models;
using datntdev.Microservices.Identity.Application.MultiTenancy.Models;
using datntdev.Microservices.ServiceDefaults.Session;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservices.Identity.Application
{
    public class IdentityApplicationDbContext(
        DbContextOptions<IdentityApplicationDbContext> options,
        AppSessionContext session) : DbContext(options)
    {
        public DbSet<AppTenantEntity> AppTenants { get; set; }

        public DbSet<AppUserEntity> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AppTenantEntity>();
            builder.Entity<AppUserEntity>(b =>
            {
                b.HasIndex(u => u.Username).IsUnique();
                b.HasIndex(u => u.EmailAddress).IsUnique();
                b.HasIndex(u => u.PasswordHash);
                b.HasMany(e => e.Tenants).WithMany(e => e.Users)
                    .UsingEntity<AppTenantUserEntity>("AppTenantUsers",
                        l => l.HasOne<AppTenantEntity>().WithMany(e => e.TenantUsers).HasForeignKey(e => e.TenantId),
                        r => r.HasOne<AppUserEntity>().WithMany(e => e.TenantUsers).HasForeignKey(e => e.UserId));
                b.HasQueryFilter(u => session.IsHostTenant || u.Tenants.Any(t => t.Id == session.TenantId));
            });
        }
    }
}
