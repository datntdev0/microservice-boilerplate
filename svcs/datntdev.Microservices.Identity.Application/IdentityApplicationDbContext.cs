using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Identity.Application.Authorization.Roles.Models;
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

        public DbSet<AppRoleEntity> AppRoles { get; set; }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SetDefaults();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            SetDefaults();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AppTenantEntity>();
            builder.Entity<AppUserEntity>(b =>
            {
                b.HasIndex(e => e.Username).IsUnique();
                b.HasIndex(e => e.EmailAddress).IsUnique();
                
                b.HasMany(e => e.Tenants).WithMany(e => e.Users)
                    .UsingEntity<AppTenantUserEntity>("AppTenantUsers",
                        l => l.HasOne<AppTenantEntity>().WithMany(e => e.TenantUsers).HasForeignKey(e => e.TenantId),
                        r => r.HasOne<AppUserEntity>().WithMany(e => e.TenantUsers).HasForeignKey(e => e.UserId));
                
                b.HasQueryFilter(u => session.IsHostTenant || u.Tenants.Any(t => t.Id == session.TenantId));
            });

            builder.Entity<AppUserClaimEntity>(b =>
            {
                b.HasOne(e => e.User).WithMany(e => e.Claims).HasForeignKey(e => e.UserId);
                b.HasQueryFilter(u => u.TenantId == session.TenantId);
            });

            builder.Entity<AppRoleEntity>(b =>
            {
                b.HasMany(e => e.Users).WithMany(e => e.Roles)
                    .UsingEntity<AppRoleUserEntity>("AppRoleUsers",
                        l => l.HasOne<AppUserEntity>().WithMany(e => e.RoleUsers).HasForeignKey(e => e.UserId),
                        r => r.HasOne<AppRoleEntity>().WithMany(e => e.RoleUsers).HasForeignKey(e => e.RoleId));

                b.HasQueryFilter(u => u.TenantId == session.TenantId);
            });

            builder.Entity<AppRoleClaimEntity>(b =>
            {
                b.HasOne(e => e.Role).WithMany(e => e.Claims).HasForeignKey(e => e.RoleId);
                b.HasQueryFilter(u => u.TenantId == session.TenantId);
            });
        }

        private void SetDefaults()
        {
            var changedEntities = ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged);
            foreach (var entry in changedEntities)
            {
                if (session.TenantId.HasValue && entry.Entity is ITenancyEntity entity)
                {
                    entity.TenantId = session.TenantId.Value;
                }
            }

            foreach (var entry in changedEntities.Where(x => x.State == EntityState.Modified))
            {
                if (entry.Entity is IAuditUpdatedEntity entity)
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                    entity.UpdatedBy = session.UserInfo?.Username;
                }
            }

            foreach (var entry in changedEntities.Where(x => x.State == EntityState.Added))
            {
                if (entry.Entity is IAuditCreatedEntity entity)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.CreatedBy = session.UserInfo?.Username;
                }
            }
        }
    }
}
