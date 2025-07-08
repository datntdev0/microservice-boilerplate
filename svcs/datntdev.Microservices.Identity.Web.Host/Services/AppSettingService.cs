using datntdev.Microservices.Identity.Web.Host.Models;

namespace datntdev.Microservices.Identity.Web.Host.Services
{
    public class AppSettingService
    {
        public const string CookieName = "AppSettings";

        public AppSettingModel AppSetting { get; set; } = AppSettingModel.Default;
    }
}
