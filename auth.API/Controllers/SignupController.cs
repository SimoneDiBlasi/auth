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

        public readonly ISignup signup;
        public SignupController(ISignup signup)
        {
            this.signup = signup;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("by-token")]
        public async Task<IActionResult> SetTokenSignupHandler([FromBody] Signup request)
        {
            var results = await signup.SignupHandler(request);
            if (results.IsSuccessfull)
                return Ok("User correctly registrated");
            return BadRequest(results.Errors);
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string emailToken)
        {
            var result = await signup.ConfirmEmail(userId, emailToken);
            if (result) return Ok("Email confermata");
            return BadRequest();
        }

    }
}
