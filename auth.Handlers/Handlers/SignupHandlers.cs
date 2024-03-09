using auth.Core.Interfaces;
using auth.Core.Models.Signup;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace auth.Handlers.Login
{
    public class SignupHandlers : ISignup
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IEmail email;
        public readonly IConfiguration configuration;

        public SignupHandlers(IConfiguration configuration, UserManager<IdentityUser> userManager, IEmail email)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.email = email;
        }

        public async Task<SignupResponse> Signup(Signup request)
        {

            var isValidEmail = request.Email != null && email.IsValidEmail(request.Email);
            if (!isValidEmail) return new SignupResponse() { Successful = false, Errors = new List<string>() { "Email not valid" }, EmailToken = null, UserId = null };

            var user = new IdentityUser
            {
                Email = request.Email,
                UserName = request.Username
            };

            if (request.Password == null || request.Password != request.ConfirmPassword)
                return new SignupResponse() { Successful = false, Errors = new List<string>() { "Password not valid" }, EmailToken = null, UserId = null };

            //In fututo crea una nuova tabella che salva gli indirizzi 
            var result = await userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                var emailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                // 
                await email.SendRegistrationEmail(user.Email ?? string.Empty, user.Id, emailToken);

                return new SignupResponse() { Successful = result.Succeeded, Errors = new List<string>(), EmailToken = emailToken, UserId = user.Id };
            }

            var errors = result.Errors.Select(val => val.Description).ToList();
            return new SignupResponse() { Successful = result.Succeeded, EmailToken = null, Errors = errors, UserId = null };

        }
    }
}
