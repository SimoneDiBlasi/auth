using auth.Core.Interfaces;
using auth.Core.Models.Signup;
using auth.Handlers.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace auth.Handlers.Login
{
    public class SignupHandlers : ISignup
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IEmail email;
        public readonly IConfiguration configuration;
        public readonly AuthDbContext db;
        public readonly IRole _role;



        public SignupHandlers(IConfiguration configuration, UserManager<IdentityUser> userManager, IEmail email, AuthDbContext db, IRole _role)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.email = email;
            this.db = db;
            this._role = _role;

        }

        public async Task<SignupResponse> SignupAsync(Signup request)
        {

            var isValidEmail = request.Email != null && email.IsValidEmail(request.Email);
            if (!isValidEmail) return new SignupResponse() { Successful = false, Errors = new List<string>() { "Email not valid" }, EmailToken = null, UserId = null };

            var user = new IdentityUser { Email = request.Email, UserName = request.Username };

            if (request.Password == null || request.Password != request.ConfirmPassword)
                return new SignupResponse() { Successful = false, Errors = new List<string>() { "Password not valid" }, EmailToken = null, UserId = null };


            var result = await userManager.CreateAsync(user, request.Password); // gestire in una classe
            if (result.Succeeded)
            {
                if (request.Address != null && (request.Address.City != null || request.Address.PostalCode != null || request.Address.State != null || request.Address.Country != null || request.Address.Street != null))
                {

                    var address = new Model.Address()
                    {
                        User = user,
                        UserId = user.Id,
                        City = request.Address.City,
                        PostalCode = request.Address.PostalCode,
                        State = request.Address.State,
                        Country = request.Address.Country,
                        Street = request.Address.Street,
                    };

                    await db.Address.AddAsync(address);
                }

                if (request != null && request.Role != null)
                {
                    await _role.AddRoleAsync(request.Role.ToString() ?? string.Empty);
                    await userManager.AddToRoleAsync(user, request.Role.ToString() ?? string.Empty);
                }
                if (request != null && request.Claims != null)
                {
                    var claims = request.Claims.Select(val => new Claim(val.Key.ToString(), val.Value)); // gestire i claims in una classe e creare un dominio dei claims
                    await userManager.AddClaimsAsync(user, claims);
                }
                var emailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                await email.SendEmailWithTokenAsync(user.Email ?? string.Empty, user.Id, emailToken);
                await db.SaveChangesAsync();
                return new SignupResponse() { Successful = result.Succeeded, Errors = new List<string>(), EmailToken = emailToken, UserId = user.Id };
            }

            var errors = result.Errors.Select(val => val.Description).ToList();
            return new SignupResponse() { Successful = result.Succeeded, EmailToken = null, Errors = errors, UserId = null };

        }
    }
}
