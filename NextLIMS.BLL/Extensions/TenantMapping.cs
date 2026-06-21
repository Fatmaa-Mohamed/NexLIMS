using NextLIMS.BLL.DTO;
using NextLIMS.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.Extensions
{
    public static class TenantMapping
    {
        public static TenantDTO ToDTO(this Tenant tenant)
        {
            return new TenantDTO
            {
                Id = tenant.Id,
                Name = tenant.Name,
                Slug = tenant.Slug,
                IsActive = tenant.IsActive ?? false,
                Location = tenant.Location,
                SubscriptionStatus = tenant.SubscriptionStatus,
                SubscriptionStartDate = tenant.SubscriptionStartDate,
                SubscriptionEndDate = tenant.SubscriptionEndDate,
                SamplesUsedThisMonth = tenant.SamplesUsedThisMonth,
                MonthlySampleLimit = tenant.MonthlySampleLimit,
                CreatedAt = tenant.CreatedAt,
                PlanName = tenant.SubscriptionTier
            };
        }
    }
}