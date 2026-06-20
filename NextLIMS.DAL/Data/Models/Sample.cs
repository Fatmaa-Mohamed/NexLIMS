namespace NextLIMS.DAL.Data.Models
{
    public class Sample
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int ClientId { get; set; }
        public string SampleName { get; set; }
        public string SampleType { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }

        public Tenant Tenant { get; set; }
        public Client Client { get; set; }
        public ICollection<SampleTest> SampleTests { get; set; }
        public ICollection<SampleWorkflow> SampleWorkflows { get; set; }
        public ICollection<AuditLog> AuditLogs { get; set; }
    }
}
