using auth.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth.API.Controllers
{
    [ApiController]
    [Route("api/logout")]
    public class LogoutController : ControllerBase
    {

        public readonly ILogout logout;
        public LogoutController(ILogout logout)
        {
            this.logout = logout;
        }

        [HttpGet]
        [Authorize]
        [Route("by-cookie")]
        public async Task<IActionResult> LogoutByCookie()
        {
            var isProcessed = await logout.LogoutByCookie();
            return isProcessed ? Ok("Logout successfull") : UnprocessableEntity();

        }

        [HttpGet]
        [Authorize]
        [Route("by-token")]
        public async Task<IActionResult> LogoutToken()
        {
            await logout.LogoutByToken();
            return Ok();
        }
    }
}
