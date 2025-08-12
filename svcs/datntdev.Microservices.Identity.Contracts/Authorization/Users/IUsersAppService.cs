using datntdev.Microservices.Common.Application;
using datntdev.Microservices.Identity.Contracts.Authorization.Users.Dtos;

namespace datntdev.Microservices.Identity.Contracts.Authorization.Users
{
    public interface IUsersAppService : 
        IApplicationService<long, UserDto, UserListDto, UserListRequest, UserCreateRequest, UserUpdateRequest>
    {
    }
}
