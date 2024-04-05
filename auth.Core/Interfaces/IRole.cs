using Microsoft.AspNetCore.Identity;

namespace auth.Core.Interfaces
{
    public interface IRole
    {
        public Task<IdentityRole> GetRoleAsync(string roleName);
        public Task<List<IdentityRole>> GetRolesAsync();
        public Task AddRoleAsync(string roleName);
        public Task<bool> UpdateRoleAsync(string roleName, string newRole);
        public Task<bool> DeleteRoleAsync(string roleName);
    }
}
