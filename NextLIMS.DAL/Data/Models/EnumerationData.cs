namespace NextLIMS.DAL.Data.Models
{
    public class EnumerationData
    {
        public int Id { get; set; }
        public int SampleTestId { get; set; }
        public int TenantId { get; set; }
        public decimal? Weight { get; set; }
        public decimal? DiluentAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }

        public SampleTest SampleTest { get; set; }
        public Tenant Tenant { get; set; }
        public ICollection<EnumerationDilution> EnumerationDilutions { get; set; }
    }
}
