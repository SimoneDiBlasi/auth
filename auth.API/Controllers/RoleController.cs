using auth.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

        /// <summary>
        /// Gets a role by its name.
        /// </summary>
        /// <param name="userId">The user ID for authorization.</param>
        /// <param name="roleName">The name of the role to retrieve.</param>
        /// <returns>The role information if found, otherwise returns a 403 Forbidden response.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("role/{roleName}")]
        [ProducesResponseType(typeof(IdentityRole), 200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetRoleByName(string userId, string roleName)
        {
            var authorizationResult = await authorizationService.AuthorizeAsync(User, userId, new CustomRequirement(userId));
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }
            var role = await _role.GetRoleAsync(roleName);
            return role != null ? Ok(role) : NotFound();

        }

        /// <summary>
        /// Gets all roles.
        /// </summary>
        /// <param name="userId">The user ID for authorization.</param>
        /// <returns>A list of all roles if the user is authorized, otherwise returns a 403 Forbidden response.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("roles")]
        [ProducesResponseType(typeof(List<IdentityRole>), 200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetRoles(string userId)
        {
            var authorizationResult = await authorizationService.AuthorizeAsync(User, userId, new CustomRequirement(userId));
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }
            var roles = await _role.GetRolesAsync();
            return roles.Any() ? Ok(roles) : NotFound();
        }

        /// <summary>
        /// Adds a new role.
        /// </summary>
        /// <param name="userId">The user ID for authorization.</param>
        /// <param name="roleName">The name of the role to add.</param>
        /// <returns>An HTTP status code indicating the success of the operation.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("role/{roleName}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> AddRole(string userId, string roleName)
        {
            var authorizationResult = await authorizationService.AuthorizeAsync(User, userId, new CustomRequirement(userId));
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            await _role.AddRoleAsync(roleName);
            return Ok();
        }

        /// <summary>
        /// Updates an existing role.
        /// </summary>
        /// <param name="userId">The user ID for authorization.</param>
        /// <param name="roleName">The name of the role to update.</param>
        /// <param name="newRole">The new role name.</param>
        /// <returns>An HTTP status code indicating the success of the operation.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("role/{roleName}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> UpdateRole(string userId, string roleName, string newRole)
        {
            var authorizationResult = await authorizationService.AuthorizeAsync(User, userId, new CustomRequirement(userId));
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var isUpdated = await _role.UpdateRoleAsync(roleName, newRole);
            return isUpdated ? Ok() : BadRequest();

        }

        /// <summary>
        /// Deletes a role.
        /// </summary>
        /// <param name="userId">The user ID for authorization.</param>
        /// <param name="roleName">The name of the role to delete.</param>
        /// <returns>An HTTP status code indicating the success of the operation.</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("role/{roleName}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> DeleteRole(string userId, string roleName)
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
