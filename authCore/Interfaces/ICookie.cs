using auth.Core.Models.Cookie;

namespace auth.Core.Interfaces
{
    public interface ICookie
    {
        public Task<UserDataCookie> SetCookieAuthenticationHandler();
    }
}
