using AutoMapper;
using datntdev.Microservices.Common.Application;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.ServiceDefaults.Application
{
    public class BaseApplicationService(IServiceProvider services) : IApplicationService
    {
        protected IMapper _mapper { get; set; } = services.GetRequiredService<IMapper>();
    }
}
