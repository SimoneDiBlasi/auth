using auth.Core.Interfaces;
using auth.Core.Models.Login;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace auth.Handlers.Login
{
    public class AuthenticationHandlers : ILogin
    {

        private readonly IHttpContextAccessor context;
        public readonly IConfiguration configuration;
        public readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        public AuthenticationHandlers(IHttpContextAccessor context, IConfiguration configuration, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }


        public async Task<bool> SetCookieAuthenticationHandler(string username, string password)
        {

            if (username == "admin" && password == "password")
            {
                // Se l'autenticazione ha successo, crea i claim per l'utente
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, username),
                new Claim("Admin", "true"), // Aggiungi il claim 'admin' per l'utente admin
                new Claim("DeveloperExperienceYear","6")
                // Altri claim se necessario
                };
                await CreateSessionCookie(claims);
                return true;
            }

            return false;

        }

        public async Task<Token?> SetTokenAuthenticationHandler(Credential credential)
        {

            var user = await userManager.FindByNameAsync(credential.Username);
            if (user == null) return new Token { AccessToken = null, ExpiresAt = null, Errors = "Failed to login" };
            var result = await signInManager.PasswordSignInAsync(user, credential.Password, false, true);
            if (result.IsLockedOut)
                return new Token() { AccessToken = null, ExpiresAt = null, Errors = "User is lockout, just wait" };

            if (result.Succeeded)
            {
                // Se l'autenticazione ha successo, crea i claim per l'utente
                var claims = new List<Claim>()
                {
                new Claim("UserId", user.Id),
                new Claim("Admin", "true"), // Aggiungi il claim 'admin' per l'utente admin
                new Claim("DeveloperExperienceYear","6")
                // Altri claim se necessario
                };

                var expiresAt = DateTime.UtcNow.AddMinutes(10);
                var token = new Token()
                {
                    AccessToken = CreateToken(claims, expiresAt),
                    ExpiresAt = expiresAt,
                };

                return token;
            }

            return new Token { AccessToken = null, ExpiresAt = null, Errors = "Failed to login" }; ;

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

        private string CreateToken(IEnumerable<Claim> claims, DateTime expireAt)
        {
            var secretKey = Encoding.ASCII.GetBytes(configuration.GetValue<string>("SecretKey") ?? "");
            var jwt = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expireAt,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature
                    ));


            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}

