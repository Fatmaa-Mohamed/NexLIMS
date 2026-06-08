using static System.Net.Mime.MediaTypeNames;

namespace NexLIMS.API.Data.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<TenantDepartment> TenantDepartments { get; set; }
        public ICollection<Test> Tests { get; set; }
    }
}
