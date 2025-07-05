using datntdev.Microservices.Identity.Application.Authorization.Roles;
using datntdev.Microservices.Identity.Application.Authorization.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservices.Identity.Application.Repositories.Data
{
    internal class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext<AppUserEntity, AppRoleEntity, long>(options)
    {
    }
}
