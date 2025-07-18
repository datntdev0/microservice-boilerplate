using datntdev.Microservices.Identity.Application.Authorization.Users.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservices.Identity.Application.Authorization.Users
{
    public class UserManager(IdentityApplicationDbContext dbContext)
    {
        public Task<AppUserEntity?> FindAsync(string username, CancellationToken ct)
        {
            return dbContext.AppUsers.FirstOrDefaultAsync(u => u.Username == username, ct);
        }

        public Task<IdentityResult> CreateAsync(AppUserEntity user, string password, CancellationToken ct)
        {
            user.PasswordHash = new PasswordHasher<AppUserEntity>().HashPassword(user, password);
            dbContext.AppUsers.Add(user);
            return dbContext.SaveChangesAsync(ct).ContinueWith(t => IdentityResult.Success, ct);
        }
    }
}
