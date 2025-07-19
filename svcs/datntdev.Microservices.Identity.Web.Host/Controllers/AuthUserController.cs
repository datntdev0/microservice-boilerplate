using datntdev.Microservices.Identity.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;


namespace datntdev.Microservices.Identity.Web.Host.Controllers
{
    [ApiController]
    [Route("me")]
    public class AuthUserController() : ControllerBase
    {
        [HttpPost("signout")]
        public async Task<IActionResult> SignOutAsync()
        {
            await HttpContext.SignOutAsync();
            return Redirect(Constants.SignInPath);
        }
    }
}
