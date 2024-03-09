namespace auth.Core.Models.Signup
{
    public class Signup
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }

    }
}
