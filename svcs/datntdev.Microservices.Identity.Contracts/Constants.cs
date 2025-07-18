using Microsoft.AspNetCore.Authentication.Cookies;

namespace datntdev.Microservices.Identity.Contracts
{
    public class Constants
    {
        public const string AdminUsername = "admin@datntdev.com";
        public const string AdminPassword = "123Qwe!@#";

        public const string AuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        public const string SignInPath = "/auth/signin";
        public const string SignUpPath = "/auth/signup";
        public const string SignOutPath = "/auth/signout";
        public const string OAuthAuthEndpoint = "/connect/authorize";
        public const string OAuthTokenEndpoint = "/connect/token";
    }
}
