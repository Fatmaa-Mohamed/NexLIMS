namespace NexLIMS.API.Data.Models
{
    public class Test
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string TestName { get; set; }
        public string TestType { get; set; }
        public string StandardMethod { get; set; }
        public int? TurnaroundTime { get; set; }

        public Department Department { get; set; }
        public ICollection<TenantTest> TenantTests { get; set; }
        public ICollection<ConfirmationTestTemplate> ConfirmationTestTemplates { get; set; }

    }
}
