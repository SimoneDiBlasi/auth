namespace auth.Core.Interfaces
{
    public interface IRecovery
    {
        public Task<bool> SendEmailRecoveryPassword(string userId);
    }
}
