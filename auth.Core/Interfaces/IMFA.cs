using Microsoft.AspNetCore.Identity;

namespace auth.Core.Interfaces
{
    public interface IMFA
    {
        public Task<bool> MultiFactorAuthenticationEmailAsync(IdentityUser user);
        public Task<bool> UseOTPCodeByEmail(string securityCode);

    }
}
