using datntdev.Microservices.Common.Application;
using datntdev.Microservices.Identity.Contracts.MultiTenancy.Dtos;

namespace datntdev.Microservices.Identity.Contracts.MultiTenancy
{
    public interface ITenantsAppService :
        IApplicationService<long, TenantDto, TenantListDto, TenantListRequest, TenantCreateRequest, TenantUpdateRequest>
    {
    }
}
