namespace datntdev.Microservice.Identity.Web.Host.Models
{
    public class AppSettingModel
    {
        public required string Theme { get; set; }

        public static AppSettingModel Default => new()
        {
            Theme = "light"
        };
    }
}
