namespace auth.Core.Interfaces
{
    public interface ILogout
    {
        public Task<bool> LogoutByCookie();
    }
}
