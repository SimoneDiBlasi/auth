using auth.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth.API.Controllers
{
    [ApiController]
    [Route("api/logout")]
    public class LoginController : ControllerBase
    {

        public readonly ILogin login;
        public LoginController(ILogin login)
        {
            this.login = login;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("by-cookie")]
        public IActionResult SetCookieAuthenticationHandler(string username, string password)
        {
            var results = login.SetCookieAuthenticationHandler(username, password);
            return Ok(results);
        }


        [Authorize(Policy = "SeniorDeveloperPolicy")]
        [HttpGet]
        [Route("policy")]

        public IActionResult AmISeniorDeveloper()
        {
            return Ok("si sono un senior developer");
        }
    }
}
