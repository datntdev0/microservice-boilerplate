using datntdev.Microservices.Identity.Application.Authorization.Users;
using datntdev.Microservices.Identity.Application.MultiTenancy;
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
                .SetTenantInfo(AppSessionTenancyInfo.HostTenant);

            await EnsureOpenIddictApplicationExistsAsync(cancellationToken);
            await EnsureDefaultTenantExistsAsync(cancellationToken);
        }

        private async Task EnsureDefaultTenantExistsAsync(CancellationToken cancellationToken)
        {
            var tenantManager = services.GetRequiredService<AppTenantManager>();

            // Check if the default tenant exists, if not, create it.
            var tenant = await tenantManager.GetAppTenantAsync(Constants.HostTenantId);
            if (tenant == null)
            {
                tenant = new AppTenantEntity
                {
                    Name = Constants.HostTenantName,
                    Description = "Default Tenant for Host",
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
