namespace auth.Core.Models.Login
{
    public class Token
    {
        public string? AccessToken { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public string? Errors { get; set; }
    }
}
