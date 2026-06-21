using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.DAL.Data.Models
{
    public class SubscriptionPlan
    {
        public int Id { get; set; } 
        public string PlanName { get; set; } 
        public decimal Price { get; set; }
        public int MonthlySampleLimit { get; set; }
        public ICollection<Tenant> Tenants { get; set; }
    }
}
