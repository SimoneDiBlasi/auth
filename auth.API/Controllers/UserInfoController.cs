using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace auth.API.Controllers
{
    [ApiController]
    [Route("api/userinfo")]

    public class UserInfoController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IAuthorizationService authorizationService;

        public UserInfoController(UserManager<IdentityUser> userManager, IAuthorizationService authorizationService)
        {
            this.userManager = userManager;
            this.authorizationService = authorizationService;
        }

        /// <summary>
        /// Retrieves user information by user ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>The user information if the user is authorized, otherwise returns a 403 Forbidden response.</returns>
        [Authorize]
        [HttpGet]
        [Route("/{id}")]
        [ProducesResponseType(typeof(IdentityUser), 200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserInfo(string id)
        {
            var authorizationResult = await authorizationService.AuthorizeAsync(User, id, new CustomRequirement(id));
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }
            var user = await userManager.FindByIdAsync(id);
            return user != null ? Ok(user) : NotFound();
        }
    }
}
