using datntdev.Microservices.Identity.Application.Authorization.Users;
using Microsoft.AspNetCore.Identity;
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
            await EnsureOpenIddictApplicationExistsAsync(cancellationToken);
            await EnsureDefaultAdminUserExistsAsync(cancellationToken);
        }

        private async Task EnsureOpenIddictApplicationExistsAsync(CancellationToken cancellationToken)
        {
            var openIddictConfiguration = _configuration.GetSection("OpenIddict");
            var openIddictClientId = openIddictConfiguration.GetValue<string>("ClientId");
            var openIddictClientSecret = openIddictConfiguration.GetValue<string>("ClientSecret");
            var openIddictRedirectUris = openIddictConfiguration.GetValue<string>("RedirectUris");

            if (string.IsNullOrEmpty(openIddictClientId)) return;

            var manager = services.GetRequiredService<IOpenIddictApplicationManager>();

            // TODO: To make it simple, we delete the existing application if it exists.
            var application = await manager.FindByClientIdAsync(openIddictClientId, cancellationToken);
            if (application != null) await manager.DeleteAsync(application, cancellationToken);

            // Create a new OpenIddict application with the specified client ID.
            // The application type is set to Web, and the client type is set to Public.
            var newApplication = new OpenIddictApplicationDescriptor
            {
                DisplayName = $"{openIddictClientId}.Client",
                ClientId = $"{openIddictClientId}.Client",
                ClientType = OpenIddictConstants.ClientTypes.Public, 
                ApplicationType = OpenIddictConstants.ApplicationTypes.Web,
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.Token,

                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,

                    OpenIddictConstants.Permissions.ResponseTypes.Code,
                }
            };

            openIddictRedirectUris?.Split(",").Select(x => new Uri(x))
                .ToList().ForEach(x => newApplication.RedirectUris.Add(x));

            await manager.CreateAsync(newApplication, cancellationToken);
        }

        private async Task EnsureDefaultAdminUserExistsAsync(CancellationToken cancellationToken)
        {
            var userManager = services.GetRequiredService<UserManager<AppUserEntity>>();

            // Check if the default admin user exists, if not, create it.
            var adminUser = await userManager.FindByNameAsync("admin@datntdev.com");
            if (adminUser == null)
            {
                adminUser = new AppUserEntity
                {
                    UserName = "admin@datntdev.com",
                    Email = "admin@datntdev.com",
                    FirstName = "Admin",
                    LastName = "System",
                };
                await userManager.CreateAsync(adminUser, "123Qwe!@#");
            }
        }
    }
}
