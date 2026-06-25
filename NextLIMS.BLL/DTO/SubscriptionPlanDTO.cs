using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.DTO
{
    public class SubscriptionPlanDTO
    {
        public int Id { get; set; }
        public string PlanName { get; set; }
        public decimal Price { get; set; }
        public int MonthlySampleLimit { get; set; }
    }
}
