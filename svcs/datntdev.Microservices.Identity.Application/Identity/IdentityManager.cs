using datntdev.Microservices.Identity.Application.Authorization.Users;
using datntdev.Microservices.Identity.Application.Authorization.Users.Models;
using datntdev.Microservices.Identity.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using IdentityResult = datntdev.Microservices.Identity.Application.Identity.Models.IdentityResult;

namespace datntdev.Microservices.Identity.Application.Identity
{
    public class IdentityManager(IServiceProvider services)
    {
        private readonly UserManager _userManager = services.GetRequiredService<UserManager>();
        private readonly IHttpContextAccessor _contextAccessor = services.GetRequiredService<IHttpContextAccessor>();
        private readonly IdentityApplicationDbContext _dbContext = services.GetRequiredService<IdentityApplicationDbContext>();
        private readonly PasswordHasher<AppUserEntity> _passwordHasher = services.GetRequiredService<PasswordHasher<AppUserEntity>>();

        public async Task<IdentityResult> SignInWithPassword(string username, string password, CancellationToken ct)
        {
            // Ignore query filters to allow querying all users cross tenants
            var user = await _dbContext.AppUsers.IgnoreQueryFilters()
                .FirstOrDefaultAsync(e => e.Username == username, ct);
            if (user == null) return IdentityResult.Failure;

            var passwordVerificationResult = _passwordHasher
                .VerifyHashedPassword(user, user.PasswordHash, password);

            if (passwordVerificationResult == PasswordVerificationResult.Success)
            {
                var claims = new Claim[]
                {
                    new(ClaimTypes.Name, user.Username),
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Email, user.EmailAddress ?? string.Empty),
                    new(ClaimTypes.GivenName, user.FirstName ?? string.Empty),
                    new(ClaimTypes.Surname, user.LastName ?? string.Empty),
                };
                var claimsIdentity = new ClaimsIdentity(claims, Constants.AuthenticationScheme);
                await _contextAccessor.HttpContext!.SignInAsync(new ClaimsPrincipal(claimsIdentity));
            }

            return passwordVerificationResult switch
            {
                PasswordVerificationResult.Success => IdentityResult.Success,
                PasswordVerificationResult.Failed => IdentityResult.Failure,
                _ => throw new NotImplementedException(),
            };
        }

        public async Task<IdentityResult> SignUpWithPassword(AppUserEntity user, string password, CancellationToken ct)
        {
            // Ignore query filters to allow querying all users cross tenants
            var existingUser = await _dbContext.AppUsers.IgnoreQueryFilters()
                .FirstOrDefaultAsync(e => e.Username == user.Username, ct);
            if (existingUser != null) return IdentityResult.DuplicatedUsername;

            await _userManager.CreateAsync(user, password, ct);
            return IdentityResult.Success;
        }
    }
}
