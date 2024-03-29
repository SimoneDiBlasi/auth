using auth.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace auth.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class RoleController : ControllerBase
    {
        private readonly IRole _role;
        private readonly IAuthorizationService authorizationService;


        public RoleController(IRole _role, IAuthorizationService authorizationService)
        {
            this._role = _role;
            this.authorizationService = authorizationService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("role/{roleName}")]
        public async Task<IActionResult> GetRoleByName(string userId, string roleName)
        {
            var authorizationResult = await authorizationService.AuthorizeAsync(User, userId, new CustomRequirement(userId));
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }
            var role = await _role.GetRoleAsync(roleName);
            return Ok(role);

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("roles")]
        public async Task<IActionResult> GetRoles(string userId)
        {
            var authorizationResult = await authorizationService.AuthorizeAsync(User, userId, new CustomRequirement(userId));
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }
            var roles = await _role.GetRolesAsync();
            return Ok(roles);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("role/{roleName}")]
        public async Task<IActionResult> addRole(string userId, string roleName)
        {
            var authorizationResult = await authorizationService.AuthorizeAsync(User, userId, new CustomRequirement(userId));
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            await _role.AddRoleAsync(roleName);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("role/{roleName}")]
        public async Task<IActionResult> updateRole(string userId, string roleName, string newRole)
        {
            var authorizationResult = await authorizationService.AuthorizeAsync(User, userId, new CustomRequirement(userId));
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var isUpdated = await _role.UpdateRoleAsync(roleName, newRole);
            return isUpdated ? Ok() : BadRequest();

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("role/{roleName}")]
        public async Task<IActionResult> deleteRole(string userId, string roleName)
        {
            var authorizationResult = await authorizationService.AuthorizeAsync(User, userId, new CustomRequirement(userId));
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var isDeleted = await _role.DeleteRoleAsync(roleName);
            return isDeleted ? Ok() : BadRequest();

        }
    }
}
