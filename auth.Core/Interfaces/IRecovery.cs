namespace auth.Core.Interfaces
{
    public interface IRecovery
    {
        public Task<bool> SendEmailRecoveryPasswordAsync(string userId);
        public Task<bool> ChangePasswordAsync(string userId, string newPassword, string passwordToken);
    }
}
