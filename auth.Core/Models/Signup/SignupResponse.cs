namespace auth.Core.Models.Signup
{
    public class SignupResponse
    {
        public bool IsSuccessfull { get; set; }
        public List<string>? Errors { get; set; }

    }
}
