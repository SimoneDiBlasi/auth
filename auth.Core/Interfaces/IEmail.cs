namespace auth.Core.Interfaces
{
    public interface IEmail
    {
        public bool IsValidEmail(string email);
        public Task SendRegistrationEmail(string email, string userId, string emailToken);
        public Task<bool> ConfirmEmail(string userId, string emailToken);
    }
}
