using System.ComponentModel.DataAnnotations;

namespace datntdev.Microservices.Identity.Contracts.Authorization.Roles.Dtos
{
    public class RoleCreateRequest
    {
        [Required]
        public int TenantId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
