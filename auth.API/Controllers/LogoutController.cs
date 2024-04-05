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


        /// <summary>
        /// Logs out the currently authenticated user.
        /// </summary>
        /// <returns>An HTTP status code indicating the success of the operation.</returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 401)]
        public async Task<IActionResult> Logout()
        {
            await logout.LogoutAsync();
            return Ok();
        }
    }
}
