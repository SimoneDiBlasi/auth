using auth.Core.Interfaces;
using auth.Core.Models.Signup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace auth.Handlers.Login
{
    public class SignupHandlers : ISignup
    {

        private readonly IHttpContextAccessor context;
        private readonly UserManager<IdentityUser> userManager;
        public readonly IConfiguration configuration;

        public SignupHandlers(IHttpContextAccessor context, IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.configuration = configuration;
            this.userManager = userManager;
        }

        public async Task<SignupResponse> SignupHandler(Signup request)
        {

            //Validare mail
            var isValidEmail = request.Email != null ? IsValidEmail(request.Email) : false;
            if (!isValidEmail) return new SignupResponse() { IsSuccessfull = false, Errors = new List<string>() { "Email not valid" } };
            //create user
            var user = new IdentityUser
            {
                Email = request.Email,
                UserName = request.Username
            };

            if (request.Password == null || request.Password != request.ConfirmPassword)
                return new SignupResponse() { IsSuccessfull = false, Errors = new List<string>() { "Password not valid" } };

            //In fututo crea una nuova tabella che salva gli indirizzi 
            var result = await userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
                return new SignupResponse() { IsSuccessfull = result.Succeeded, Errors = new List<string>() };

            var errors = result.Errors.Select(val => val.Description).ToList();
            return new SignupResponse() { IsSuccessfull = result.Succeeded, Errors = errors };

        }

        private static bool IsValidEmail(string email)
        {
            // Regular expression pattern for validating email addresses
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            // Create a Regex object
            Regex regex = new Regex(pattern);

            // Use the IsMatch method to check if the email matches the pattern
            return regex.IsMatch(email);
        }

    }

}
