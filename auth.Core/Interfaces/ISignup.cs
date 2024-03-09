using auth.Core.Models.Signup;

namespace auth.Core.Interfaces
{
    public interface ISignup
    {
        public Task<SignupResponse> SignupHandler(Signup request);
        public Task<bool> ConfirmEmail(string userId, string emailToken);
    }
}
