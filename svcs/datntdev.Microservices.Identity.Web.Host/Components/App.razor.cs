using datntdev.Microservices.ServiceDefaults.Session;
using Microsoft.AspNetCore.Components;

namespace datntdev.Microservices.Identity.Web.Host.Components
{
    public partial class App
    {
        [Inject]
        private AppSessionContext AppSession { get; set; } = default!;
    }
}