namespace NextLIMS.DAL.Data.Models
{
    public class TenantTest
    {
        public int Id { get; set; }
        public int ?TenantId { get; set; }
        public int ?TestId { get; set; }
        public bool IsActive { get; set; }
        public decimal? Price { get; set; }
        public string ?StandardMethod { get; set; }
        public int? TurnaroundTime { get; set; }
        public string ?SupportedSampleTypes { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }

        public Tenant? Tenant { get; set; }
        public Test ?Test { get; set; }
        public ICollection<SampleTest>? SampleTests { get; set; }
    }
}
