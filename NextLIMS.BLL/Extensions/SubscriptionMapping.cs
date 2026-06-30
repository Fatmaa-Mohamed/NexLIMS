using NextLIMS.BLL.DTO;
using NextLIMS.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.Extensions
{
    public static class SubscriptionMapping
    {
        public static SubscriptionDetailsDTO SubscriptionDetails(this Tenant tenant)
        {
            return new SubscriptionDetailsDTO()
            {
                SubscriptionTier = tenant.SubscriptionTier,
                SamplesUsedThisMonth = tenant.SamplesUsedThisMonth,
                MonthlySampleLimit = tenant.MonthlySampleLimit,
                SubscriptionEndDate = tenant.SubscriptionEndDate,
                SubscriptionStartDate = tenant.SubscriptionStartDate,
                SubscriptionStatus = tenant.SubscriptionStatus,
            };
        }
    }
}
