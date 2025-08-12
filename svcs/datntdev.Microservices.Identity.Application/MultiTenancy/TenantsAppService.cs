using datntdev.Microservices.Common.Application.Dtos;
using datntdev.Microservices.Identity.Contracts.MultiTenancy;
using datntdev.Microservices.Identity.Contracts.MultiTenancy.Dtos;
using datntdev.Microservices.ServiceDefaults.Application;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Identity.Application.MultiTenancy
{
    public class TenantsAppService(IServiceProvider services) 
        : BaseApplicationService(services), ITenantsAppService
    {
        private readonly TenantManager _manager = services.GetRequiredService<TenantManager>();

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

        public async Task<GetListResponse<TenantListDto>> GetListAsync(TenantListRequest input)
        {
            var (count, items) = await _manager.GetAppTenantsAsync(input);

            var result = new GetListResponse<TenantListDto>
            {
                TotalCount = count,
                Items = _mapper.Map<List<TenantListDto>>(items)
            };

            return result;           
        }

        public Task<TenantDto> UpdateAsync(TenantUpdateRequest input)
        {
            throw new NotImplementedException();
        }
    }
}
