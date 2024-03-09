using auth.Core.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace auth.Handlers.Logout
{
    public class LogoutHandlers : ILogout
    {
        private readonly IHttpContextAccessor context;
        public LogoutHandlers(IHttpContextAccessor context)
        {
            this.context = context;
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
    }
}
