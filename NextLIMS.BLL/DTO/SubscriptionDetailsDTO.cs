using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.DTO
{
    public class SubscriptionDetailsDTO
    {
        public string SubscriptionTier { get; set; }
        public string SubscriptionStatus { get; set; }
        public DateOnly? SubscriptionStartDate { get; set; }
        public DateOnly? SubscriptionEndDate { get; set; }
        public int SamplesUsedThisMonth { get; set; }
        public int? MonthlySampleLimit { get; set; }
    }
}
