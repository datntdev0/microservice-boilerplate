using datntdev.Microservices.Identity.Application;
using datntdev.Microservices.Identity.Application.Authorization.Roles;
using datntdev.Microservices.Identity.Application.Authorization.Users;
using datntdev.Microservices.Identity.Application.Authorization.Users.Models;
using datntdev.Microservices.Identity.Application.MultiTenancy;
using datntdev.Microservices.Identity.Application.MultiTenancy.Models;
using datntdev.Microservices.Identity.Contracts;
using datntdev.Microservices.ServiceDefaults.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;

namespace datntdev.Microservices.Migrator.Seeders
{
    internal class IdentityDataSeeder(IServiceProvider services) : IDataSeeder
    {
        private readonly IConfigurationRoot _configuration = services.GetRequiredService<IConfigurationRoot>();
        private readonly AppSessionContext _sessionContext = services.GetRequiredService<AppSessionContext>();
        private readonly IdentityApplicationDbContext _dbContext = services.GetRequiredService<IdentityApplicationDbContext>();

        public async Task SeedDataAsync(CancellationToken cancellationToken)
        {
            _sessionContext.SetUserInfo(new() { Username = "Migrator" });
            await EnsureOpenIddictApplicationExistsAsync(cancellationToken);
            await EnsureDefaultAdminUserExistsAsync(cancellationToken);
            await EnsureDefaultTenantExistsAsync(cancellationToken);
        }

        private async Task EnsureDefaultAdminUserExistsAsync(CancellationToken ct)
        {
            var adminUser = _dbContext.AppUsers.IgnoreQueryFilters()
                .FirstOrDefault(x => x.Username == Constants.AdminUsername);
            if (adminUser == null)
            {
                adminUser = new AppUserEntity
                {
                    Username = Constants.AdminUsername,
                    EmailAddress = Constants.AdminUsername,
                };

                await services.GetRequiredService<UserManager>()
                    .CreateAsync(adminUser, Constants.AdminPassword, ct);
            }
        }

        private async Task EnsureDefaultTenantExistsAsync(CancellationToken ct)
        {
            var userManager = services.GetRequiredService<UserManager>();
            var roleManager = services.GetRequiredService<RoleManager>();
            var dbContext = services.GetRequiredService<IdentityApplicationDbContext>();

            var adminUser = _dbContext.AppUsers.IgnoreQueryFilters()
                .FirstOrDefault(x => x.Username == Constants.AdminUsername);

            // Check if the default tenant exists, if not, create it.
            var tenant = _dbContext.AppTenants.IgnoreQueryFilters()
                .FirstOrDefault(x => x.Id == Common.Constants.Tenancy.HostTenantId);
            if (tenant == null)
            {
                tenant = new AppTenantEntity
                {
                    Name = "Default Tenant for Host",
                    Description = "Default Tenant for Host",
                    Users = [adminUser]
                };
                await services.GetRequiredService<TenantManager>().CreateAsync(tenant, ct);
            }

            // Set the session context with the tenant and user information.
            _sessionContext.SetTenantInfo(new() { TenantId = Common.Constants.Tenancy.HostTenantId });

            // Check if the default roles exist, if not, create them.
            var userRole = await roleManager.FindAsync(Constants.UserRole, ct);
            if (userRole == null)
            {
                userRole = RoleManager.CreateUserRole(tenant.Id);
                await roleManager.CreateAsync(userRole, ct);
            }
            var adminRole = await roleManager.FindAsync(Constants.AdminRole, ct);
            if (adminRole == null)
            {
                adminRole = RoleManager.CreateAdminRole(tenant.Id, adminUser);
                await roleManager.CreateAsync(adminRole, ct);
            }
        }

        private async Task EnsureOpenIddictApplicationExistsAsync(CancellationToken ct)
        {
            var openIddictConfiguration = _configuration.GetSection("OpenIddict");
            var openIddictClientId = openIddictConfiguration.GetValue<string>("ClientId");
            var openIddictClientSecret = openIddictConfiguration.GetValue<string>("ClientSecret");
            var openIddictRedirectUris = openIddictConfiguration.GetValue<string>("RedirectUris");

            ArgumentNullException.ThrowIfNull(openIddictClientId, nameof(openIddictClientId));
            ArgumentNullException.ThrowIfNull(openIddictClientSecret, nameof(openIddictClientSecret));

            var manager = services.GetRequiredService<IOpenIddictApplicationManager>();

            // Create a new OpenIddict application with the specified client ID.
            // The application type is set to Web, and the client type is set to Public.
            var newApplication = CreatePublicApplication(openIddictClientId, openIddictRedirectUris);
            if (await manager.FindByClientIdAsync(newApplication.ClientId!, ct) == null)
                await manager.CreateAsync(newApplication, ct);

            // Create a new OpenIddict application for the confidential client.
            // The application type is set to Web, and the client type is set to Confidential.
            newApplication = CreateConfidentialApplication(
                openIddictClientId, openIddictClientSecret, openIddictRedirectUris);
            if (await manager.FindByClientIdAsync(newApplication.ClientId!, ct) == null)
                await manager.CreateAsync(newApplication, ct);
        }

        private static OpenIddictApplicationDescriptor CreatePublicApplication(
            string clientId, string? redirectUris)
        {
            var application = new OpenIddictApplicationDescriptor
            {
                DisplayName = $"{clientId}.Public",
                ClientId = $"{clientId}.Public",
                ClientType = OpenIddictConstants.ClientTypes.Public,
                ApplicationType = OpenIddictConstants.ApplicationTypes.Web,
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.ResponseTypes.Code,
                }
            };
            redirectUris?.Split(",").Select(x => new Uri(x))
                .ToList().ForEach(x => application.RedirectUris.Add(x));
            return application;
        }

        private static OpenIddictApplicationDescriptor CreateConfidentialApplication(
            string clientId, string clientSecret, string? redirectUris)
        {
            var application = new OpenIddictApplicationDescriptor
            {
                DisplayName = $"{clientId}.Confidential",
                ClientId = $"{clientId}.Confidential",
                ClientSecret = clientSecret,
                ClientType = OpenIddictConstants.ClientTypes.Confidential,
                ApplicationType = OpenIddictConstants.ApplicationTypes.Web,
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                }
            };
            redirectUris?.Split(",").Select(x => new Uri(x))
                .ToList().ForEach(x => application.RedirectUris.Add(x));
            return application;
        }
    }
}
