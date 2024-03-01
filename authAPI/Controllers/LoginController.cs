using auth.Core.Interfaces;
using auth.Core.Models.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth.API.Controllers
{
    [ApiController]
    [Route("api/login")]
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
            return results != null ? Ok(results) : Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("by-token")]
        public IActionResult SetTokenAuthenticationHandler([FromBody] Credential credential)
        {
            var token = login.SetTokenAuthenticationHandler(credential);
            return Ok(token);
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
