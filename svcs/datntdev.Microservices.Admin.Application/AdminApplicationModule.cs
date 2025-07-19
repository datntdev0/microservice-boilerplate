using datntdev.Microservices.Admin.Contracts;
using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.ServiceDefaults;

namespace datntdev.Microservices.Admin.Application
{
    [DependOn(typeof(AdminContractsModule), typeof(ServiceDefaultModule))]
    public class AdminApplicationModule : BaseModule
    {

    }
}
