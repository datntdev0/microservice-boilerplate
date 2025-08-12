using datntdev.Microservices.Common.Models;

namespace datntdev.Microservices.Identity.Contracts.Authorization.Users.Dtos
{
    public class UserDto : BaseAuditDto<long>
    {
        public string Username { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = [];
        public List<string> Tenants { get; set; } = [];
    }
}
