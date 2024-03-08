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
        public async Task<IActionResult> SetCookieAuthenticationHandler([FromBody] Signup request)
        {
            var results = await signup.SignupHandler(request);
            if (results.IsSuccessfull)
                return Ok("User correctly registrated");
            return BadRequest(results.Errors);
        }

    }
}
