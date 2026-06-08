using System.Data;

namespace NexLIMS.API.Data.Models
{
    public class Tenant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string SubscriptionTier { get; set; }
        public string SubscriptionStatus { get; set; }
        public DateOnly? SubscriptionStartDate { get; set; }
        public DateOnly? SubscriptionEndDate { get; set; }
        public int SamplesUsedThisMonth { get; set; }
        public int? MonthlySampleLimit { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<Role> Roles { get; set; }
        public ICollection<Client> Clients { get; set; }
        public ICollection<TenantDepartment> TenantDepartments { get; set; }
        public ICollection<TenantTest> TenantTests { get; set; }
        public ICollection<Sample> Samples { get; set; }
        public ICollection<SampleWorkflow> SampleWorkflows { get; set; }
        public ICollection<AuditLog> AuditLogs { get; set; }
    }
}
