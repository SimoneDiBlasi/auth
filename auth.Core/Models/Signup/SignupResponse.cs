namespace auth.Core.Models.Signup
{
    public class SignupResponse
    {
        public bool IsSuccessfull { get; set; }
        public List<string>? Errors { get; set; }
        public string? EmailToken { get; set; }
        public string? UserId { get; set; }

    }
}
