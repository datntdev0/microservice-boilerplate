using datntdev.Microservices.Common.Modular;
using Microsoft.IdentityModel.Tokens;

namespace datntdev.Microservices.Admin.Web.Host
{
    public class AdminWebHostModule : BaseModule
    {
        public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
        {
            services.AddOpenIddictValidation(configs);
        }
    }

    internal static class AdminWebHostModuleExtensions
    {
        public static IServiceCollection AddOpenIddictValidation(
            this IServiceCollection services, IConfigurationRoot configs)
        {
            var encryptionKey = Convert.FromBase64String(configs["OpenIddict:EncryptionKey"]!);

            services.AddOpenIddict()
                .AddValidation(options =>
                {
                    options.SetIssuer(configs["OpenIddict:Issuer"]!);
                    options.AddEncryptionKey(new SymmetricSecurityKey(encryptionKey));
                    options.UseSystemNetHttp();
                    options.UseAspNetCore();
                });

            return services;
        }
    }
}
