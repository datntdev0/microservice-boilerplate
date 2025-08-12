using System.ComponentModel.DataAnnotations;

namespace datntdev.Microservices.Identity.Contracts.MultiTenancy.Dtos
{
    public class TenantCreateRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
