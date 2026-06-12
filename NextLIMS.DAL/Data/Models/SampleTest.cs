namespace NextLIMS.DAL.Data.Models
{
    public class SampleTest
    {
        public int Id { get; set; }
        public int ?SampleId { get; set; }
        public int ?TenantTestId { get; set; }
        public int? AssignedToUserId { get; set; }
        public string ?Status { get; set; }
        public string ?Result { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }

        public Sample ?Sample { get; set; }
        public TenantTest? TenantTest { get; set; }
        public User ?AssignedToUser { get; set; }
        public User? ApprovedByUser { get; set; }
        public EnumerationData? EnumerationData { get; set; }
        public DetectionData ?DetectionData { get; set; }
        public ICollection<SampleConfirmationTest>? SampleConfirmationTests { get; set; }
        public ICollection<AuditLog>? AuditLogs { get; set; }
    }
}
