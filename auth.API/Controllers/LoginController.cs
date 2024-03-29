using auth.Core.Interfaces;
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
        [Route("validate-credential")]
        public async Task<IActionResult> VerifyCredential(string email, string password)
        {
            var userId = await login.VerifyCredentialAsync(email, password);
            return Ok(userId);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("login/{otp}")]
        public async Task<IActionResult> Login(string userId, string otp)
        {
            var token = await login.LoginAsync(userId, otp);
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
