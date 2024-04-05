using auth.Core.Interfaces;
using auth.Core.Models.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace auth.Handlers.Recovery
{
    public class RecoveryHandler : IRecovery
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IEmail _email;

        public RecoveryHandler(UserManager<IdentityUser> userManager, IConfiguration configuration, IEmail _email)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this._email = _email;
        }

        public async Task<bool> ChangePasswordAsync(string userId, string newPassword, string passwordToken)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return false;


            var isTokenValid = await userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword", passwordToken);
            if (!isTokenValid)
                return false;


            var result = await userManager.ResetPasswordAsync(user, passwordToken, newPassword);
            if (result.Succeeded)
                return true;
            return false;
        }

        public async Task<bool> SendEmailRecoveryPasswordAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null) return false;
            var passwordToken = await userManager.GeneratePasswordResetTokenAsync(user);
            if (passwordToken == null)
                return false;
            var passwordRecoveyLink = $"https://www.google.com/"; // Questo comando dovrei reindirizzare verso una pagina frontend che al momento non c'è
            var from = configuration["SMTP:User"] ?? string.Empty;
            var to = user.Email ?? string.Empty;
            var subject = "Here your OTP Code";
            var body = EmailLayout.PasswordRecoveryLayoutEmail(passwordRecoveyLink);
            await _email.SendEmailAsync(from, to, subject, body);
            return true;

        }
    }
}





