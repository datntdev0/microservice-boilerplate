using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;


namespace datntdev.Microservices.Identity.Web.Host.Controllers
{
    [ApiController]
    [Route("api/me")]
    public class AuthUserController() : ControllerBase
    {
        [HttpPost("signout")]
        public async Task<IActionResult> SignOutAsync()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/auth/signin");
        }
    }
}
