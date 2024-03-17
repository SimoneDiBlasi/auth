using auth.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace auth.Handlers.Handlers
{
    public class MFAHandlers : IMFA
    {
        private readonly UserManager<IdentityUser> userManager;
        public readonly SignInManager<IdentityUser> signInManager;
        private readonly IEmail emailService;
        private readonly IConfiguration configuration;

        public MFAHandlers(UserManager<IdentityUser> userManager, IEmail emailService, IConfiguration configuration, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }


        public async Task MultiFactorAuthenticationEmail(string email)
        {
            var user = await userManager.FindByNameAsync(email);
            if (user == null)
                return;
            var securityCode = await userManager.GenerateTwoFactorTokenAsync(user, "Email");
            await emailService.SendEmailAsync(configuration["SMTP:User"] ?? string.Empty, email, "MyWebApp's OTP", $"Please use this code as the OTP: {securityCode}");
        }


        public async Task UseOTPCodeByEmail(string securityCode)
        {
            await signInManager.TwoFactorSignInAsync("Email", securityCode, false, false);
        }

        //crea quello per inviare al telefono 

    }
}
