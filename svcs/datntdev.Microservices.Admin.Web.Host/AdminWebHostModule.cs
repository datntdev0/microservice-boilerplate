using datntdev.Microservices.Admin.Application;
using datntdev.Microservices.Common.Modular;
using Microsoft.IdentityModel.Tokens;

namespace datntdev.Microservices.Admin.Web.Host
{
    [DependOn(typeof(AdminApplicationModule))]
    public class AdminWebHostModule : BaseModule
    {
        public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
        {
            ConfigureAuthentication(services, configs);
        }

        private static void ConfigureAuthentication(IServiceCollection services, IConfigurationRoot configs)
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
        }
    }
}
