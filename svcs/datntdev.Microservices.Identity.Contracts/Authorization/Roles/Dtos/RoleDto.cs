using datntdev.Microservices.Common.Models;

namespace datntdev.Microservices.Identity.Contracts.Authorization.Roles.Dtos
{
    public class RoleDto : BaseAuditDto<long>
    {
        public int TenantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Users { get; set; } = [];
        public List<string> Claims { get; set; } = [];
    }
}
