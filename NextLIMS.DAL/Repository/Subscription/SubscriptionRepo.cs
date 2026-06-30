using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.DAL.Repository.Subscription
{
    public class SubscriptionRepo
    {
        private readonly ApplicationDbContext _dbContext;

        public SubscriptionRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Tenant?> GetTenantDetails(int TenantId)
        {
            return await _dbContext.Tenants
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(t => t.Id == TenantId);
        }
        public async Task<IReadOnlyList<SubscriptionPlan?>> GetSubscriptionPlans()
        {
            return await _dbContext.SubscriptionPlans.ToListAsync();
        }
        public async Task<SubscriptionPlan> GetSubscriptionPlan(int id)
        {
            return await _dbContext.SubscriptionPlans.FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
