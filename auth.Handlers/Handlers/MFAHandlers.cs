using auth.Core.Interfaces;
using auth.Core.Models.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace auth.Handlers.Handlers
{
    public class MFAHandlers : IMFA
    {
        private readonly UserManager<IdentityUser> userManager;
        public readonly SignInManager<IdentityUser> signInManager;
        private readonly IEmail emailService;
        private readonly IConfiguration configuration;
        private readonly ILogger<MFAHandlers> logger;

        public MFAHandlers(UserManager<IdentityUser> userManager, IEmail emailService, IConfiguration configuration, SignInManager<IdentityUser> signInManager, ILogger<MFAHandlers> logger)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.logger = logger;
        }


        public async Task<bool> MultiFactorAuthenticationEmailAsync(IdentityUser user)
        {
            if (user == null)
                return false;
            var securityCode = await userManager.GenerateTwoFactorTokenAsync(user, "Email");
            if (securityCode == null) return false;
            try
            {
                var bodyLayout = new EmailLayout();
                var from = configuration["SMTP:User"] ?? string.Empty;
                var to = user.Email ?? string.Empty;
                var subject = "Here your OTP Code";
                var body = EmailLayout.OTPLayoutEmail(securityCode);
                await emailService.SendEmailAsync(from, to, subject, body);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while sending multi-factor authentication email.");
                return false;
            }
        }

        public async Task<bool> UseOTPCodeByEmailAsync(string securityCode)
        {
            var task = await signInManager.TwoFactorSignInAsync("Email", securityCode, false, false);
            if (task.Succeeded)
            {
                return true;
            }
            return false;
        }
    }
}

