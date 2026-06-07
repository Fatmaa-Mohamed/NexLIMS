namespace NexLIMS.API.Data.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int? SampleId { get; set; }
        public int? SampleTestId { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Reason { get; set; }
        public DateTime Timestamp { get; set; }

        public Tenant Tenant { get; set; }
        public Sample Sample { get; set; }
        public SampleTest SampleTest { get; set; }
        public User User { get; set; }
    }
}
