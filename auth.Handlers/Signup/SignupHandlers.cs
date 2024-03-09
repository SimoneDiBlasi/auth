using auth.Core.Interfaces;
using auth.Core.Models.Signup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
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

            var isValidEmail = request.Email != null ? IsValidEmail(request.Email) : false;
            if (!isValidEmail) return new SignupResponse() { IsSuccessfull = false, Errors = new List<string>() { "Email not valid" }, EmailToken = null, UserId = null };
            //create user
            var user = new IdentityUser
            {
                Email = request.Email,
                UserName = request.Username
            };

            if (request.Password == null || request.Password != request.ConfirmPassword)
                return new SignupResponse() { IsSuccessfull = false, Errors = new List<string>() { "Password not valid" }, EmailToken = null, UserId = null };

            //In fututo crea una nuova tabella che salva gli indirizzi 
            var result = await userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                var emailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                // 
                await SendEmail(user.Email ?? string.Empty, user.Id, emailToken);

                return new SignupResponse() { IsSuccessfull = result.Succeeded, Errors = new List<string>(), EmailToken = emailToken, UserId = user.Id };
            }

            var errors = result.Errors.Select(val => val.Description).ToList();
            return new SignupResponse() { IsSuccessfull = result.Succeeded, EmailToken = null, Errors = errors, UserId = null };

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

        private static async Task SendEmail(string email, string userId, string emailToken)
        {
            var confirmationLink = $"https://localhost:7296/api/signup/confirm-email?userId={userId}&emailToken={emailToken}";
            var message = new MailMessage("simo_dibla@hotmail.it", email, "Please confirm your mail", $"Per confermare il tuo account, <a href='{confirmationLink}'>clicca qui</a>");

            using (var emailClient = new SmtpClient("smtp-relay.brevo.com", 587))
            {
                emailClient.Credentials = new NetworkCredential("simo_dibla@hotmail.it", "FY6c5n1C2B3ZbjTz");
                await emailClient.SendMailAsync(message);
            }
        }

        public async Task<bool> ConfirmEmail(string userId, string emailToken)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return false;
            var confirmEmail = await userManager.ConfirmEmailAsync(user, emailToken);
            if (confirmEmail.Succeeded) return true;
            return false;
        }
    }

}
