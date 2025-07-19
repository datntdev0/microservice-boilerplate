using datntdev.Microservices.Common.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace datntdev.Microservices.Identity.Application.Authorization.Users.Models
{
    [Table("AppUserClaims")]
    public class AppUserClaimEntity : BaseAuditEntity<long>, ITenancyEntity
    {
        public int TenantId { get; set; }
        public long UserId { get; set; }
        public string ClaimType { get; set; } = string.Empty;
        public string ClaimValue { get; set; } = string.Empty;

        public AppUserEntity User { get; set; } = null!;
    }
}
