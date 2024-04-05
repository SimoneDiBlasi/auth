using auth.Core.Models.Signup;

namespace auth.Core.Interfaces
{
    public interface ISignup
    {
        public Task<SignupResponse> SignupAsync(Signup request);
    }
}
