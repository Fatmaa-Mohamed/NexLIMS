namespace NextLIMS.DAL.Data.Models
{
    public class SampleType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<TestSampleType> TestSampleTypes { get; set; }

        public ICollection<TenantTestSampleType> TenantTestSampleTypes { get; set; }
    }
}
