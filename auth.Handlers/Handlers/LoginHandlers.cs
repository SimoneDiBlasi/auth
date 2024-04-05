using auth.Core.Interfaces;
using auth.Core.Models.Login;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace auth.Handlers.Login
{
    public class LoginHandlers : ILogin
    {
        public readonly IConfiguration configuration;
        public readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IMFA mfa;

        public LoginHandlers(IConfiguration configuration, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IMFA mfa)
        {
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.mfa = mfa;
        }

        public async Task<string> VerifyCredentialAsync(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return "Impossible to log in";

            var claims = await userManager.GetClaimsAsync(user);
            if (!claims.Any())
                return "Impossible to log in";

            var isMFA = await mfa.MultiFactorAuthenticationEmailAsync(user);
            if (isMFA)
            {
                var result = await signInManager.PasswordSignInAsync(user, password, false, true);
                if (result.IsLockedOut)
                    return "Account is locked, you have to wait";
                await userManager.SetTwoFactorEnabledAsync(user, true);
                return user.Id;
            }

            return "Impossible to log in";
        }


        public async Task<Token> LoginAsync(string userId, string otp)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return new Token { AccessToken = null, ExpiresAt = null, Errors = "Failed to login" }; ;

            var claims = await userManager.GetClaimsAsync(user);
            if (!claims.Any())
                return new Token { AccessToken = null, ExpiresAt = null, Errors = "Failed to login" }; ;

            var isCorrect = await mfa.UseOTPCodeByEmailAsync(otp);
            if (isCorrect)
            {
                var expiresAt = DateTime.UtcNow.AddHours(2);
                var token = new Token()
                {
                    AccessToken = CreateToken(claims, expiresAt),
                    ExpiresAt = expiresAt,
                };
                return token;
            }

            return new Token { AccessToken = null, ExpiresAt = null, Errors = "Failed to login, code otp is wrong" };

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


