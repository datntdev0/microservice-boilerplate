using datntdev.Microservices.Common.Models;

namespace datntdev.Microservices.Identity.Contracts.Authorization.Users.Dtos
{
    public class UserListDto : BaseAuditDto<long>
    {
        public string Username { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
