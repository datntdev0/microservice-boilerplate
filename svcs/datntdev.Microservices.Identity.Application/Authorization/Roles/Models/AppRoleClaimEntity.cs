using datntdev.Microservices.Common.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace datntdev.Microservices.Identity.Application.Authorization.Roles.Models
{
    [Table("AppRoleClaims")]
    public class AppRoleClaimEntity : BaseAuditEntity<long>, ITenancyEntity
    {
        public int TenantId { get; set; }
        public long RoleId { get; set; }
        public string ClaimType { get; set; } = string.Empty;
        public string ClaimValue { get; set; } = string.Empty;

        public AppRoleEntity Role { get; set; } = null!;
    }
}
