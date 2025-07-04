using Microsoft.AspNetCore.Components;

namespace datntdev.Microservice.Identity.Web.Host.Components
{
    public partial class App
    {
        [Inject]
        private Services.AppSettingService AppSettingService { get; set; } = default!;

        private Shared.SweetAlertModal? _refSweetAlertModal { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        }
    }
}