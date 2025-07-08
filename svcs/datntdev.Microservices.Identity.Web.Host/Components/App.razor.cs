using Microsoft.AspNetCore.Components;

namespace datntdev.Microservices.Identity.Web.Host.Components
{
    public partial class App
    {
        [Inject]
        private Services.AppSettingService AppSettingService { get; set; } = default!;
    }
}