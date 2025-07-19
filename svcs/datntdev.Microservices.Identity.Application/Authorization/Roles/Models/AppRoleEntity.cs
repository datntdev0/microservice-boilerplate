using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Identity.Application.Authorization.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservices.Identity.Application.Authorization.Roles.Models
{
    [Index(nameof(TenantId), nameof(Name), IsUnique = true)]
    public class AppRoleEntity : BaseAuditEntity<long>, ITenancyEntity
    {
        public int TenantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public List<AppUserEntity> Users { get; set; } = [];
        public List<AppRoleUserEntity> RoleUsers { get; set; } = [];
    }
}
