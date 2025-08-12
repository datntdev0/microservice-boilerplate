using datntdev.Microservices.Common.Models;

namespace datntdev.Microservices.Identity.Contracts.MultiTenancy.Dtos
{
    public class TenantListDto : BaseAuditDto<long>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
