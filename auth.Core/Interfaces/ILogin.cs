using auth.Core.Models.Login;

namespace auth.Core.Interfaces
{
    public interface ILogin
    {
        public Task<string> VerifyCredentialHandler(string email, string password);
        public Task<Token> LoginHandler(string userId, string otp);
    }
}
