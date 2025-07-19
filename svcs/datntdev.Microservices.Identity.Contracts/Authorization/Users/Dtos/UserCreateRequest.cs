namespace datntdev.Microservices.Identity.Contracts.Authorization.Users.Dtos
{
    public class UserCreateRequest
    {
        public string Username { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
    }
}
