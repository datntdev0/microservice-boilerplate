using datntdev.Microservices.Common.Models;

namespace datntdev.Microservices.Identity.Contracts.Authorization.Roles.Dtos
{
    public class RoleListDto : BaseAuditDto<long>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
