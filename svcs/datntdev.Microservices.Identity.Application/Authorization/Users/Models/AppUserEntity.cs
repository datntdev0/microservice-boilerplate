using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Identity.Application.Authorization.Roles.Models;
using datntdev.Microservices.Identity.Application.MultiTenancy.Models;

namespace datntdev.Microservices.Identity.Application.Authorization.Users.Models
{
    public class AppUserEntity : FullAuditEntity<long>
    {
        public string Username { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public List<AppTenantEntity> Tenants { get; set; } = [];
        public List<AppTenantUserEntity> TenantUsers { get; set; } = [];
        public List<AppRoleEntity> Roles { get; set; } = [];
        public List<AppRoleUserEntity> RoleUsers { get; set; } = [];
        public List<AppUserClaimEntity> Claims { get; set; } = [];
    }
}
