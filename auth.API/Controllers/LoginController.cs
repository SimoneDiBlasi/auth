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

        /// <summary>
        /// Verifies user credentials.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>User ID if credentials are valid.</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("validate-credential")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> VerifyCredential(string email, string password)
        {
            var userId = await login.VerifyCredentialAsync(email, password);
            return Ok(userId);
        }

        /// <summary>
        /// Logs in a user using their user ID and one-time password (OTP).
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <param name="otp">The one-time password (OTP).</param>
        /// <returns>The authentication token if login is successful.</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("{otp}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Login(string userId, string otp)
        {
            var token = await login.LoginAsync(userId, otp);
            return token != null ? Ok(token) : BadRequest();
        }
    }
}
