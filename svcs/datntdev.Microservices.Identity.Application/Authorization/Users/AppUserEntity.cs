using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Identity.Application.MultiTenancy;
using Microsoft.AspNetCore.Identity;

namespace datntdev.Microservices.Identity.Application.Authorization.Users
{
    public class AppUserEntity : IdentityUser<long>, IAuditEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        public List<AppTenantEntity> Tenants { get; set; } = [];
        public List<AppTenantUserEntity> TenantUsers { get; set; } = [];
    }
}
