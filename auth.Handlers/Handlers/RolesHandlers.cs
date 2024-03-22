using auth.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Data.Entity;

namespace auth.Handlers.Handlers
{
    public class RolesHandlers : IRole
    {
        private readonly RoleManager<IdentityRole> roleManager;
        public RolesHandlers(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task AddRole(string roleName)
        {
            if (await roleManager.RoleExistsAsync(roleName) == false)
            {
                // Se il ruolo non esiste, lo creiamo utilizzando CreateAsync
                var newRole = new IdentityRole(roleName);
                await roleManager.CreateAsync(newRole);
            }
        }

        public async Task<bool> DeleteRole(string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);

            // Se il ruolo esiste, elimínalo
            if (role != null)
            {
                await roleManager.DeleteAsync(role);
                return true;
            }
            return false;
        }

        public async Task<IdentityRole> GetRole(string roleName)
        {
            return await roleManager.Roles.Where(val => val.Name == roleName).FirstOrDefaultAsync();
        }

        public async Task<List<IdentityRole>> GetRoles()
        {
            return await roleManager.Roles.ToListAsync();
        }

        public async Task<bool> UpdateRole(string roleName, string newRole)
        {
            var role = await roleManager.FindByNameAsync(roleName);

            // Se il role esiste, aggiornalo con il nuovo nome
            if (role != null)
            {
                role.Name = newRole;
                await roleManager.UpdateAsync(role);
                return true;
            }
            return false;
        }
    }
}
