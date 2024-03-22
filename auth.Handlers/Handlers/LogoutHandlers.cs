using auth.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace auth.Handlers.Logout
{
    public class LogoutHandlers : ILogout
    {
        private readonly SignInManager<IdentityUser> signInManager;

        public LogoutHandlers(SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        public async Task Logout()
        {
            await signInManager.SignOutAsync();
        }
    }
}
