using System.ComponentModel.DataAnnotations;

namespace datntdev.Microservices.Identity.Contracts.Authorization.Users.Dtos
{
    public class UserCreateRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string EmailAddress { get; set; } = string.Empty;
    }
}
