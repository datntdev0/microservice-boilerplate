using Microsoft.AspNetCore.Identity;

namespace datntdev.Microservices.Identity.Application.Authorization.Roles
{
    internal class AppRoleEntity : IdentityRole<long>
    {
        public string? Description { get; set; }
    }
}
