using auth.Core.Models.Login;

namespace auth.Core.Interfaces
{
    public interface ILogin
    {
        public Task<string> VerifyCredentialAsync(string email, string password);
        public Task<Token> LoginAsync(string userId, string otp);
    }
}
