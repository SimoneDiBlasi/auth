using auth.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace auth.API.Controllers
{
    [ApiController]
    [Route("api/recovery")]
    public class RecoveryController : ControllerBase
    {
        private readonly IRecovery recovery;

        public RecoveryController(IRecovery recovery)
        {
            this.recovery = recovery;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/email-password")]
        public async Task<IActionResult> SendEmailRecoveryPassword(string userId)
        {

            var isSent = await recovery.SendEmailRecoveryPassword(userId);
            return Ok(isSent);

        }


        [AllowAnonymous]
        [HttpGet]
        [Route("/password")]
        public async Task<IActionResult> RecoveryPassword(string userId, string passwordToken)
        {
            var isSent = await recovery.RecoveryPassword(userId, passwordToken);
            return Ok(isSent);

        }
    }
}
