namespace NextLIMS.DAL.Data.Models
{
    public class SampleWorkflow
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int SampleId { get; set; }
        public int Level { get; set; }
        public int? AssignedToId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Action { get; set; }
        public bool Flag { get; set; }
        public string Reason { get; set; }

        public Tenant Tenant { get; set; }
        public Sample Sample { get; set; }
        public User AssignedTo { get; set; }
    }
}
