using datntdev.Microservices.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace datntdev.Microservices.Identity.Application.Authorization.Users
{
    public class AppUserRoleEntity : IdentityUserRole<long>, ITenancyEntity
    {
        public int? TenantId { get; set; }
    }
}
