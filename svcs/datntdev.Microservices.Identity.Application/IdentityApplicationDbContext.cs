using datntdev.Microservices.Identity.Application.Authorization.Roles;
using datntdev.Microservices.Identity.Application.Authorization.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservices.Identity.Application
{
    public class IdentityApplicationDbContext(DbContextOptions<IdentityApplicationDbContext> options)
        : IdentityDbContext<AppUserEntity, AppRoleEntity, long>(options)
    {
    }
}
