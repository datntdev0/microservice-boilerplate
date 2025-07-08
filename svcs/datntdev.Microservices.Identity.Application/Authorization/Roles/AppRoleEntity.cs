using Microsoft.AspNetCore.Identity;

namespace datntdev.Microservices.Identity.Application.Authorization.Roles
{
    public class AppRoleEntity : IdentityRole<long>
    {
        public string? Description { get; set; }
    }
}
