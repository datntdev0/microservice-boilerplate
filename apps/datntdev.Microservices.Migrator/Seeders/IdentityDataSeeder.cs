using datntdev.Microservices.Identity.Application.Authorization.Users;
using datntdev.Microservices.Identity.Application.Authorization.Users.Models;
using datntdev.Microservices.Identity.Application.MultiTenancy;
using datntdev.Microservices.Identity.Application.MultiTenancy.Models;
using datntdev.Microservices.Identity.Contracts;
using datntdev.Microservices.ServiceDefaults.Session;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;

namespace datntdev.Microservices.Migrator.Seeders
{
    internal class IdentityDataSeeder(IServiceProvider services) : IDataSeeder
    {
        private readonly IConfigurationRoot _configuration = services.GetRequiredService<IConfigurationRoot>();

        public async Task SeedDataAsync(CancellationToken cancellationToken)
        {
            services.GetRequiredService<AppSessionContext>()
                .SetTenantInfo(new() { TenantId = Common.Constants.Tenancy.HostTenantId });

            await EnsureOpenIddictApplicationExistsAsync(cancellationToken);
            await EnsureDefaultAdminUserExistsAsync(cancellationToken);
            await EnsureDefaultTenantExistsAsync(cancellationToken);
        }

        private async Task EnsureDefaultAdminUserExistsAsync(CancellationToken cancellationToken)
        {
            var userManager = services.GetRequiredService<UserManager>();
            var adminUser = await userManager.FindAsync(Constants.AdminUsername, cancellationToken);
            if (adminUser == null)
            {
                adminUser = new AppUserEntity
                {
                    Username = Constants.AdminUsername,
                    EmailAddress = Constants.AdminUsername,
                };
                await userManager.CreateAsync(adminUser, Constants.AdminPassword, cancellationToken);
            }
        }

        private async Task EnsureDefaultTenantExistsAsync(CancellationToken cancellationToken)
        {
            var tenantManager = services.GetRequiredService<AppTenantManager>();
            var userManager = services.GetRequiredService<UserManager>();

            // Check if the default tenant exists, if not, create it.
            var tenant = await tenantManager.GetAppTenantAsync(Common.Constants.Tenancy.HostTenantId);
            if (tenant == null)
            {
                var adminUser = await userManager.FindAsync(Constants.AdminUsername, cancellationToken);
                tenant = new AppTenantEntity
                {
                    Name = "Default Tenant for Host",
                    Description = "Default Tenant for Host",
                    Users = [adminUser!]
                };
                await tenantManager.CreateAsync(tenant, cancellationToken);
            }
        }

        private async Task EnsureOpenIddictApplicationExistsAsync(CancellationToken cancellationToken)
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
            if (await manager.FindByClientIdAsync(newApplication.ClientId!, cancellationToken) == null)
                await manager.CreateAsync(newApplication, cancellationToken);

            // Create a new OpenIddict application for the confidential client.
            // The application type is set to Web, and the client type is set to Confidential.
            newApplication = CreateConfidentialApplication(
                openIddictClientId, openIddictClientSecret, openIddictRedirectUris);
            if (await manager.FindByClientIdAsync(newApplication.ClientId!, cancellationToken) == null)
                await manager.CreateAsync(newApplication, cancellationToken);
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
