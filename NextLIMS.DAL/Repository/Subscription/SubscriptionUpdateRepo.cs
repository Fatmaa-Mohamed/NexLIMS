using Microsoft.EntityFrameworkCore;
using NextLIMS.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.DAL.Repository.Subscription
{
    public class SubscriptionUpdateRepo
    {
        private readonly ApplicationDbContext _dbContext;

        public SubscriptionUpdateRepo(ApplicationDbContext  dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> AddQuota(int tenantId , int Sample)
        {
            var tenant = await _dbContext.Tenants.FirstOrDefaultAsync(t => t.Id==tenantId);
            tenant.MonthlySampleLimit += Sample;
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<int> UpgrdeSubscription(int tenantId, int SubscriptionId)
        {
            var tenant = await _dbContext.Tenants.FirstOrDefaultAsync(t => t.Id == tenantId);
            var Subscription = await _dbContext.SubscriptionPlans.FirstOrDefaultAsync(s => s.Id == SubscriptionId);
            var today = DateTime.UtcNow;
            tenant.SubscriptionStartDate = DateOnly.FromDateTime(today);
            tenant.SubscriptionEndDate = DateOnly.FromDateTime(today.AddMonths(1));
            tenant.SubscriptionTier = Subscription.PlanName;
            tenant.MonthlySampleLimit = Subscription.MonthlySampleLimit;
            tenant.SamplesUsedThisMonth = 0;
            tenant.SubscriptionPlanId = Subscription.Id;
            tenant.SubscriptionStatus = "Active";
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpgradeKeepRemainingAsync(int tenantId, int subscriptionId)
        {
            var tenant = await _dbContext.Tenants.FirstOrDefaultAsync(t => t.Id == tenantId);
            var plan   = await _dbContext.SubscriptionPlans.FirstOrDefaultAsync(s => s.Id == subscriptionId);
            var today  = DateTime.UtcNow;

            int remaining = (tenant.MonthlySampleLimit ?? 0) - tenant.SamplesUsedThisMonth;

            tenant.SubscriptionStartDate = DateOnly.FromDateTime(today);
            tenant.SubscriptionEndDate   = DateOnly.FromDateTime(today.AddMonths(1));
            tenant.SubscriptionTier      = plan.PlanName;
            tenant.MonthlySampleLimit    = remaining + plan.MonthlySampleLimit;
            tenant.SamplesUsedThisMonth  = 0;
            tenant.SubscriptionPlanId    = plan.Id;
            tenant.SubscriptionStatus    = "Active";

            return await _dbContext.SaveChangesAsync();
        }
    }
}
