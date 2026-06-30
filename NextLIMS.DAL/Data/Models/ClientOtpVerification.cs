namespace NextLIMS.DAL.Data.Models
{
    public class ClientOtpVerification
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int ClientId { get; set; }
        public string CodeHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public bool IsUsed { get; set; }
        public int AttemptCount { get; set; }
        public int MaxAttempts { get; set; }
        public string? TwilioMessageSid { get; set; }
        public Tenant Tenant { get; set; } = null!;
        public Client Client { get; set; } = null!;
    }
}