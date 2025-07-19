using datntdev.Microservices.Common.Models;

namespace datntdev.Microservices.Identity.Application.Authorization.Roles.Models
{
    public class AppRoleUserEntity : IAuditCreatedEntity, ITenancyEntity
    {
        public int TenantId { get; set; }
        public long RoleId { get; set; }
        public long UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }
}
