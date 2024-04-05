using auth.Core.Interfaces;
using auth.Core.Models.Signup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth.API.Controllers
{
    [ApiController]
    [Route("api/signup")]
    public class SignupController : ControllerBase
    {

        public readonly IEmail email;

        public readonly ISignup signup;
        public SignupController(ISignup signup, IEmail email)
        {
            this.signup = signup;
            this.email = email;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request">The signup request containing user information.</param>
        /// <returns>An HTTP status code indicating the success of the operation.</returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(IEnumerable<string>), 400)]
        public async Task<IActionResult> Signup([FromBody] Signup request)
        {
            var results = await signup.SignupAsync(request);
            if (results.Successful)
                return Ok("User correctly registrated");
            return BadRequest(results.Errors);
        }

        /// <summary>
        /// Confirms a user's email address.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="emailToken">The email confirmation token.</param>
        /// <returns>An HTTP status code indicating the success of the operation.</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("confirm-email")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string emailToken)
        {
            var result = await email.ConfirmEmailAsync(userId, emailToken);
            if (result) return Ok("Email confermata");
            return BadRequest();
        }
    }
}
