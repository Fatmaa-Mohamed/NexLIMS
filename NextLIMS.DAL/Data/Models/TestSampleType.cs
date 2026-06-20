namespace NextLIMS.DAL.Data.Models
{
    public class TestSampleType
    {
        public int TestId { get; set; }

        public int SampleTypeId { get; set; }

        public Test Test { get; set; }

        public SampleType SampleType { get; set; }
    }
}
