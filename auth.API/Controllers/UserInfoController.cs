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

        [Authorize]
        [HttpGet]
        [Route("/{id}")]
        public async Task<IActionResult> GetUserInfo(string id)
        {
            var authorizationResult = await authorizationService.AuthorizeAsync(User, id, new CustomRequirement(id));
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }
            var user = await userManager.FindByIdAsync(id);
            return Ok(user);

        }
    }
}
