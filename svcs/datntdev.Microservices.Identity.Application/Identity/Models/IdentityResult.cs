namespace datntdev.Microservices.Identity.Application.Identity.Models
{
    public class IdentityResult
    {
        public IdentityResultStatus Status { get; private set; }
        public string ErrorMessage { get; private set; } = string.Empty;

        public static IdentityResult Success => new() { Status = IdentityResultStatus.Success };
        public static IdentityResult Failure => new() { Status = IdentityResultStatus.Failure };
        public static IdentityResult DuplicatedUsername => new() 
        {
            Status = IdentityResultStatus.DuplicatedUsername,
            ErrorMessage = "Username already exists. Please try with another username",
        };
    }

    public enum IdentityResultStatus
    {
        Success,
        Failure,
        DuplicatedUsername,
    }
}
