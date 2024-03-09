using auth.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace auth.Handlers.Email
{
    public class EmailHandler : IEmail
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;

        public EmailHandler(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        public async Task<bool> ConfirmEmail(string userId, string emailToken)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return false;
            var confirmEmail = await userManager.ConfirmEmailAsync(user, emailToken);
            if (confirmEmail.Succeeded) return true;
            return false;
        }

        public bool IsValidEmail(string email)
        {
            // Regular expression pattern for validating email addresses
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            // Create a Regex object
            Regex regex = new Regex(pattern);

            // Use the IsMatch method to check if the email matches the pattern
            return regex.IsMatch(email);
        }

        public async Task SendRegistrationEmail(string email, string userId, string emailToken)
        {
            var confirmationLink = $"https://localhost:7296/api/signup/confirm-email?userId={userId}&emailToken={emailToken}";
            var from = configuration["SMTP:User"] ?? throw new Exception("User is null or not valid");
            var to = email;
            var subject = "Please confirm your mail";
            var body = $"Per confermare il tuo account, <a href='{confirmationLink}'>clicca qui</a>";
            await SendEmail(from, to, subject, body);
        }

        private async Task SendEmail(string from, string to, string subject, string body)
        {
            var message = new MailMessage(from, to, subject, body);

            using (var emailClient = new SmtpClient(configuration["SMTP:Host"], int.Parse(configuration["SMTP:Port"] ?? throw new Exception("Port number is null or not valid"))))
            {
                emailClient.Credentials = new NetworkCredential(configuration["SMTP:User"], configuration["SMTP:API-KEY"]);
                await emailClient.SendMailAsync(message);
            }
        }
    }
}





