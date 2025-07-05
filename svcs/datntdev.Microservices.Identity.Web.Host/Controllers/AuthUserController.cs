using datntdev.Microservices.Identity.Application.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace datntdev.Microservices.Identity.Web.Host.Controllers
{
    [ApiController]
    [Route("api/me")]
    public class AuthUserController(IServiceProvider services) : ControllerBase
    {
        private readonly SignInManager<AppUserEntity> SignInManager = services.GetRequiredService<SignInManager<AppUserEntity>>();

        [HttpPost("signout")]
        public async Task<IActionResult> SignOutAsync()
        {
            await SignInManager.SignOutAsync();
            return Redirect("/auth/signin");
        }
    }
}
