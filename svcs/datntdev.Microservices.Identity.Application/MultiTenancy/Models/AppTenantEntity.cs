using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Identity.Application.Authorization.Roles.Models;
using datntdev.Microservices.Identity.Application.Authorization.Users.Models;

namespace datntdev.Microservices.Identity.Application.MultiTenancy.Models
{
    public class AppTenantEntity : BaseAuditEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public List<AppUserEntity> Users { get; set; } = [];
        public List<AppTenantUserEntity> TenantUsers { get; set; } = [];
    }
}
