using datntdev.Microservices.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace datntdev.Microservices.Identity.Application.Authorization.Roles
{
    public class AppRoleClaimEntity : IdentityRoleClaim<long>, ITenancyEntity
    {
        public int? TenantId { get; set; }
    }
}
