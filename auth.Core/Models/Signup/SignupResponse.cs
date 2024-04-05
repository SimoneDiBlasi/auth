namespace auth.Core.Models.Signup
{
    public class SignupResponse
    {
        public string? UserId { get; set; }
        public string? EmailToken { get; set; }
        public bool Successful { get; set; }
        public List<string>? Errors { get; set; }

    }
}
