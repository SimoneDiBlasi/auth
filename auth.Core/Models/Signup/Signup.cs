using auth.Core.Models.Claims;
using auth.Core.Models.Roles;
namespace auth.Core.Models.Signup
{
    public class Signup
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
        public Address? Address { get; set; }
        public EnumRoles? Role { get; set; }
        public Dictionary<ClaimsKey, string>? Claims { get; set; }

    }
}
