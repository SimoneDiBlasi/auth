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

        /// <summary>
        /// Sends an email for password recovery.
        /// </summary>
        /// <param name="userId">The user ID for which to send the recovery email.</param>
        /// <returns>True if the recovery email was sent successfully, otherwise false.</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("email-password")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> SendEmailRecoveryPassword(string userId)
        {
            var isSent = await recovery.SendEmailRecoveryPasswordAsync(userId);
            return Ok(isSent);
        }


        /// <summary>
        /// Changes the password for a user.
        /// </summary>
        /// <param name="userId">The user ID for which to change the password.</param>
        /// <param name="newPassword">The new password to set.</param>
        /// <param name="passwordToken">The token required for changing the password.</param>
        /// <returns>An HTTP status code indicating the success of the operation.</returns>

        [AllowAnonymous]
        [HttpGet]
        [Route("password")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> ChangePassword(string userId, string newPassword, string passwordToken)
        {
            var isChanged = await recovery.ChangePasswordAsync(userId, newPassword, passwordToken);
            return isChanged ? Ok() : BadRequest();
        }
    }
}
