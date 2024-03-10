using auth.Core.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace auth.Handlers.Logout
{
    public class LogoutHandlers : ILogout
    {
        private readonly IHttpContextAccessor context;
        private readonly SignInManager<IdentityUser> signInManager;

        public LogoutHandlers(IHttpContextAccessor context, SignInManager<IdentityUser> signInManager)
        {
            this.context = context;
            this.signInManager = signInManager;
        }

        public async Task<bool> LogoutByCookie()
        {
            try
            {
                await context.HttpContext.SignOutAsync("MyCookieAuth");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task LogoutByToken()
        {
            await signInManager.SignOutAsync();
        }
    }
}
