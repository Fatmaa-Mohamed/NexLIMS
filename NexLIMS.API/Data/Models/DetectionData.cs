namespace NexLIMS.API.Data.Models
{
    public class DetectionData
    {
        public int Id { get; set; }
        public int SampleTestId { get; set; }
        public int TenantId { get; set; }
        public decimal? Weight { get; set; }
        public string EnrichmentMedia { get; set; }
        public decimal? MediaAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }

        public SampleTest SampleTest { get; set; }
        public Tenant Tenant { get; set; }
    }
}
