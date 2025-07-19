namespace datntdev.Microservices.Identity.Contracts.Authorization.Users.Dtos
{
    public class UserUpdateRequest
    {
        public long Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
