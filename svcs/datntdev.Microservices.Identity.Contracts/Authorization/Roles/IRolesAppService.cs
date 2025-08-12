using datntdev.Microservices.Common.Application;
using datntdev.Microservices.Identity.Contracts.Authorization.Roles.Dtos;

namespace datntdev.Microservices.Identity.Contracts.Authorization.Roles
{
    public interface IRolesAppService :
        IApplicationService<long, RoleDto, RoleListDto, RoleListRequest, RoleCreateRequest, RoleUpdateRequest>
    {
    }
}
