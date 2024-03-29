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

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Signup([FromBody] Signup request)
        {
            var results = await signup.SignupAsync(request);
            if (results.Successful)
                return Ok("User correctly registrated");
            return BadRequest(results.Errors);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string emailToken)
        {
            var result = await email.ConfirmEmailAsync(userId, emailToken);
            if (result) return Ok("Email confermata");
            return BadRequest();
        }
    }
}
