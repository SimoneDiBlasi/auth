using auth.Core.Models.Login;

namespace auth.Core.Interfaces
{
    public interface ILogin
    {
        public Task<bool> SetCookieAuthenticationHandler(string username, string password);
        public Task<Token?> SetTokenAuthenticationHandler(Credential credential);
    }
}
