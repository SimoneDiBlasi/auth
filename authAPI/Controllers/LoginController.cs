using auth.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace auth.API.Controllers
{
    [ApiController]
    [Route("/login")]
    public class LoginController : ControllerBase
    {

        public readonly ICookie cookie;
        public LoginController(ICookie cookie)
        {
            this.cookie = cookie;
        }

        [HttpGet]
        [Route("/login/cookie")]
        public IActionResult SetCookieAuthenticationHandler()
        {
            var results = SetCookieAuthenticationHandler();

            //Response.Cookies.Append()
            return Ok(results);
        }
    }
}
