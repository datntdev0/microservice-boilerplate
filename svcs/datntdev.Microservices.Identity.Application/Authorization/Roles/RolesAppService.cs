using datntdev.Microservices.Common.Application.Dtos;
using datntdev.Microservices.Identity.Contracts.Authorization.Roles;
using datntdev.Microservices.Identity.Contracts.Authorization.Roles.Dtos;

namespace datntdev.Microservices.Identity.Application.Authorization.Roles
{
    public class RolesAppService : IRolesAppService
    {
        public Task<RoleDto> CreateAsync(RoleCreateRequest input)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<RoleDto> GetAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<GetListResponse<RoleListDto>> GetListAsync(RoleListRequest input)
        {
            throw new NotImplementedException();
        }

        public Task<RoleDto> UpdateAsync(RoleUpdateRequest input)
        {
            throw new NotImplementedException();
        }
    }
}
