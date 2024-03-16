using Microsoft.AspNetCore.Identity;

namespace auth.Core.Interfaces
{
    public interface IRole
    {
        public Task<IdentityRole> GetRole(string roleName);
        public Task<List<IdentityRole>> GetRoles();
        public Task AddRole(string roleName);
        public Task<bool> UpdateRole(string roleName, string newRole);
        public Task<bool> DeleteRole(string roleName);
    }
}
