using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Identity.Application.Authorization.Roles;
using datntdev.Microservices.Identity.Application.Authorization.Users;
using datntdev.Microservices.Identity.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace datntdev.Microservices.Identity.Application
{
    [DependOn(typeof(IdentityContractsModule))]
    public class IdentityApplicationModule : BaseModule
    {
        public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
        {
            services.AddDbContext<IdentityApplicationDbContext>(o => o.UseOpenIddict());
            services.AddIdentityServices();
            services.AddOpenIddictServices(configs);
        }
    }

    internal static class IdentityApplicationModuleExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentity<AppUserEntity, AppRoleEntity>()
                .AddSignInManager()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<IdentityApplicationDbContext>();
            return services;
        }

        public static IServiceCollection AddOpenIddictServices(
            this IServiceCollection services, IConfigurationRoot configs)
        {
            var encryptionKey = Convert.FromBase64String(configs["OpenIddict:EncryptionKey"]!);

            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                        .UseDbContext<IdentityApplicationDbContext>();
                })
                .AddServer(options =>
                {
                    // TODO: Disable access token encryption for debug
                    // edit this code for applying multi higher environments
                    options.DisableAccessTokenEncryption()
                        .AddEphemeralSigningKey()
                        .AddEncryptionKey(new SymmetricSecurityKey(encryptionKey));

                    options
                        .RequireProofKeyForCodeExchange()
                        .AllowAuthorizationCodeFlow()
                        .AllowClientCredentialsFlow();

                    options
                        .SetTokenEndpointUris("/connect/token")
                        .SetAuthorizationEndpointUris("/connect/authorize");

                    options.UseAspNetCore()
                        .EnableTokenEndpointPassthrough()
                        .EnableAuthorizationEndpointPassthrough();
                });
            return services;
        }
    }
}
