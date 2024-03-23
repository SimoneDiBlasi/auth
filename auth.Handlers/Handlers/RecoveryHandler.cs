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
        private readonly IEmail email;

        public RecoveryHandler(UserManager<IdentityUser> userManager, IConfiguration configuration, IEmail email)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.email = email;
        }

        public async Task<bool> SendEmailRecoveryPassword(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return false;
            var passwordToken = await userManager.GeneratePasswordResetTokenAsync(user);
            if (passwordToken == null)
                return false;
            var passwordRecoveyLink = $"https://localhost:7296/api/recovery/password?userId={userId}&emailToken={passwordToken}";
            var from = configuration["SMTP:User"] ?? string.Empty;
            var to = user.Email ?? string.Empty;
            var subject = "Here your OTP Code";
            var body = EmailLayout.PasswordRecoveryLayoutEmail(passwordRecoveyLink);
            await email.SendEmailAsync(from, to, subject, body);
            return true;

        }
    }
}





