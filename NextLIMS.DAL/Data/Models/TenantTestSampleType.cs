namespace NextLIMS.DAL.Data.Models
{
    public class TenantTestSampleType
    {
        public int Id { get; set; }

        public int TenantTestId { get; set; }

        public int SampleTypeId { get; set; }

        public TenantTest TenantTest { get; set; }

        public SampleType SampleType { get; set; }
    }
}
