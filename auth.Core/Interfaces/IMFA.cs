namespace auth.Core.Interfaces
{
    public interface IMFA
    {
        public Task MultiFactorAuthenticationEmail(string email);
        public Task UseOTPCodeByEmail(string securityCode);

    }
}
