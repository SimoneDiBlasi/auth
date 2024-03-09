using auth.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth.API.Controllers
{
    [ApiController]
    [Route("api/email")]
    public class EmailController : ControllerBase
    {

        public readonly IEmail email;
        public EmailController(IEmail email)
        {
            this.email = email;
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string emailToken)
        {
            var result = await email.ConfirmEmail(userId, emailToken);
            if (result) return Ok("Email confermata");
            return BadRequest();
        }

    }
}
