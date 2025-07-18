using datntdev.Microservices.Common.Models;

namespace datntdev.Microservices.Identity.Application.MultiTenancy
{
    public class AppTenantEntity : BaseAuditEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
