using datntdev.Microservices.Common.Application;
using datntdev.Microservices.Identity.Contracts.Authorization.Users.Dtos;

namespace datntdev.Microservices.Identity.Contracts.Authorization.Users
{
    public interface IUsersAppService : IApplicationService
    {
        Task<List<UserListDto>> GetListAsync(UserListRequest input);
        Task<UserDto> GetAsync(long id);
        Task<UserDto> CreateAsync(UserCreateRequest input);
        Task<UserDto> UpdateAsync(UserUpdateRequest input);
        Task DeleteAsync(long id);
    }
}
