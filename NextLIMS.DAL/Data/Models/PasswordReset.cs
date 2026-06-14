


namespace NextLIMS.DAL.Data.Models
{
    public class PasswordReset
    {
        public int Id { get; set; }
        public int ?UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; } = false;

        public User? user { get; set; }
    }
}
