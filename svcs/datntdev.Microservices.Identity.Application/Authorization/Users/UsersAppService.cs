using datntdev.Microservices.Identity.Contracts.Authorization.Users;
using datntdev.Microservices.Identity.Contracts.Authorization.Users.Dtos;

namespace datntdev.Microservices.Identity.Application.Authorization.Users
{
    public class UsersAppService : IUsersAppService
    {
        public Task<UserDto> CreateAsync(UserCreateRequest input)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> GetAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserListDto>> GetListAsync(UserListRequest input)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> UpdateAsync(UserUpdateRequest input)
        {
            throw new NotImplementedException();
        }
    }
}
