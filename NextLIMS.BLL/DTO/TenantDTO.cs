using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.DTO
{
    public class TenantDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public bool IsActive { get; set; }
        public string Location { get; set; }
        public string PlanName { get; set; }
        public string SubscriptionStatus { get; set; }
        public DateOnly? SubscriptionStartDate { get; set; }
        public DateOnly? SubscriptionEndDate { get; set; }
        public int SamplesUsedThisMonth { get; set; }
        public int? MonthlySampleLimit { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class UpdateTenantDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
    }
}
