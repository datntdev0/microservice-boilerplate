using datntdev.Microservices.Common.Application.Dtos;
using datntdev.Microservices.Identity.Contracts.MultiTenancy;
using datntdev.Microservices.Identity.Contracts.MultiTenancy.Dtos;

namespace datntdev.Microservices.Identity.Application.MultiTenancy
{
    public class TenantsAppService : ITenantsAppService
    {
        public Task<TenantDto> CreateAsync(TenantCreateRequest input)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<TenantDto> GetAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<GetListResponse<TenantListDto>> GetListAsync(TenantListRequest input)
        {
            throw new NotImplementedException();
        }

        public Task<TenantDto> UpdateAsync(TenantUpdateRequest input)
        {
            throw new NotImplementedException();
        }
    }
}
