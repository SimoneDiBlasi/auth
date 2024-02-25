using auth.Core.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace auth.Handlers.Login
{
    public class AuthenticationHandlers : ILogin
    {

        private readonly IHttpContextAccessor context;

        public AuthenticationHandlers(IHttpContextAccessor context)
        {
            this.context = context;
        }

        public async Task<bool> SetCookieAuthenticationHandler(string username, string password)
        {

            if (username == "admin" && password == "password")
            {
                // Se l'autenticazione ha successo, crea i claim per l'utente
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim("admin", "true"), // Aggiungi il claim 'admin' per l'utente admin
                new Claim("DeveloperExperienceYear","4")
                // Altri claim se necessario
            };
                await CreateSessionCookie(claims);
                return true;
            }
            return false;

        }

        private async Task CreateSessionCookie(List<Claim> claims)
        {
            // Crea un oggetto ClaimsIdentity 
            var identity = new ClaimsIdentity(claims, "MyCookieAuth");

            // Crea l'oggetto ClaimsPrincipal
            var principal = new ClaimsPrincipal(identity);

            // Crea il cookie di sessione
            await context.HttpContext.SignInAsync("MyCookieAuth", principal, new AuthenticationProperties
            {
                IsPersistent = false, // Cookie non persistente
                AllowRefresh = false, // Non consentire il refresh del cookie
            });
        }
    }
}

