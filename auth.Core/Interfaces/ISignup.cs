using auth.Core.Models.Signup;

namespace auth.Core.Interfaces
{
    public interface ISignup
    {
        public Task<SignupResponse> Signup(Signup request);
    }
}
