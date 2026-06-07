namespace NexLIMS.API.Data.Models
{
    public class TenantDepartment
    {
        public int TenantId { get; set; }
        public int DepartmentId { get; set; }
        public DateTime CreatedAt { get; set; }

        public Tenant Tenant { get; set; }
        public Department Department { get; set; }
    }
}
