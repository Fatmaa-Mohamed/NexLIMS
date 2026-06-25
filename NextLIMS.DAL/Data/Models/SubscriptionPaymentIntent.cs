namespace NextLIMS.DAL.Data.Models
{
    public class SubscriptionPaymentIntent
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string InvoiceId { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;   // "add_quota" | "renew" | "upgrade"
        public int? SampleNumber { get; set; }
        public int? PlanId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Pending";      // "Pending" | "Paid" | "Failed"
        public DateTime CreatedAt { get; set; }
    }
}
