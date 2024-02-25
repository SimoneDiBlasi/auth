namespace auth.Core.Interfaces
{
    public interface ILogin
    {
        public Task<bool> SetCookieAuthenticationHandler(string username, string password);
    }
}
