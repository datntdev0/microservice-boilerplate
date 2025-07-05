using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Identity.Contracts;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("datntdev.Microservices.Identity.Web.Host")]

namespace datntdev.Microservices.Identity.Application
{
    [DependOn(typeof(IdentityContractsModule))]
    public class IdentityApplicationModule : BaseModule
    {
    }
}
