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
        public async Task<IActionResult> Logout()
        {
            await logout.LogoutAsync();
            return Ok();
        }
    }
}
