namespace auth.Core.Interfaces
{
    public interface IEmail
    {
        public bool IsValidEmail(string email);
        public Task SendEmailWithTokenAsync(string email, string userId, string emailToken);
        public Task<bool> ConfirmEmailAsync(string userId, string emailToken);
        public Task SendEmailAsync(string from, string to, string subject, string body);
    }
}
