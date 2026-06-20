namespace NextLIMS.DAL.Data.DataSeed
{
    public class TestSeed
    {
        public int DepartmentId { get; set; }

        public int? TenantId { get; set; }

        public string TestName { get; set; }

        public string TestType { get; set; }

        public string StandardMethod { get; set; }

        public int? TurnaroundTime { get; set; }

        public List<int> SampleTypeIds { get; set; } = [];
    }
}
