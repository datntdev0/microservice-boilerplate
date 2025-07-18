using datntdev.Microservices.Identity.Application.Authorization.Users.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Identity.Application.Authorization.Users
{
    public class UserManager(IServiceProvider services)
    {
        private readonly IdentityApplicationDbContext _dbContext = services.GetRequiredService<IdentityApplicationDbContext>();
        private readonly PasswordHasher<AppUserEntity> _passwordHasher = services.GetRequiredService<PasswordHasher<AppUserEntity>>();

        public Task<AppUserEntity?> FindAsync(string username, CancellationToken ct)
        {
            return _dbContext.AppUsers.FirstOrDefaultAsync(u => u.Username == username, ct);
        }

        public Task<AppUserEntity> CreateAsync(AppUserEntity user, string password, CancellationToken ct)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, password);
            _dbContext.AppUsers.Add(user);
            return _dbContext.SaveChangesAsync(ct).ContinueWith(t => user, ct);
        }
    }
}
