using datntdev.Microservices.Identity.Application.Authorization.Permissions;
using datntdev.Microservices.Identity.Application.Authorization.Roles.Models;
using datntdev.Microservices.Identity.Application.Authorization.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservices.Identity.Application.Authorization.Roles
{
    public class RoleManager(IdentityApplicationDbContext dbContext)
    {
        public Task<AppRoleEntity?> FindAsync(string roleName, CancellationToken ct)
        {
            return dbContext.AppRoles.FirstOrDefaultAsync(r => r.Name == roleName, ct);
        }

        public Task<AppRoleEntity> CreateAsync(AppRoleEntity role, CancellationToken ct)
        {
            dbContext.AppRoles.Add(role);
            return dbContext.SaveChangesAsync(ct).ContinueWith(t => role, ct);
        }

        public static AppRoleEntity CreateAdminRole(int tenantId, AppUserEntity? user = null)
        {
            var permissions = AppPermissionResolver.GetAppPermissionEnums(AppPermissionResolver.RootPermissions);
            var claim = new AppRoleClaimEntity
            {
                TenantId = tenantId,
                ClaimType = Common.Constants.ClaimTypes.Permissions,
                ClaimValue = string.Join(",", permissions.Select(p => ((int)p).ToString())),
            };
            return new AppRoleEntity
            {
                TenantId = tenantId,
                Name = "Admin",
                Description = "Administrator role with full permissions",
                Users = user != null ? [user] : [],
                Claims = [claim],
            };
        }

        public static AppRoleEntity CreateUserRole(int tenantId, AppUserEntity? user = null)
        {
            return new AppRoleEntity
            {
                TenantId = tenantId,
                Name = "User",
                Description = "Standard user role with limited permissions",
                Users = user != null ? [user] : [],
            };
        }
    }
}
