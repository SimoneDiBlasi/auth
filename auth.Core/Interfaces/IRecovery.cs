namespace auth.Core.Interfaces
{
    public interface IRecovery
    {
        public Task<bool> SendEmailRecoveryPasswordAsync(string email);
        public Task<bool> ChangePasswordAsync(string userId, string newPassword, string passwordToken);
    }
}
