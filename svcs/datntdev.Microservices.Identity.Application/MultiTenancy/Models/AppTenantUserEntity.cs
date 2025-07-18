using datntdev.Microservices.Common.Models;

namespace datntdev.Microservices.Identity.Application.MultiTenancy.Models
{
    public class AppTenantUserEntity : IAuditCreatedEntity
    {
        public int TenantId { get; set; }
        public long UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }
}
