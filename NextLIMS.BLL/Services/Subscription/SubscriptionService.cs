using Microsoft.AspNetCore.Http;
using NextLIMS.BLL.DTO;
using NextLIMS.BLL.Extensions;
using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.Repository.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.Services.Subscription
{
    public class SubscriptionService
    {
        private readonly SubscriptionRepo _subscriptionRepo;
        private IHttpContextAccessor httpContextAccessor;

        public SubscriptionService(SubscriptionRepo subscriptionRepo , IHttpContextAccessor httpContextAccessor)
        {
            _subscriptionRepo = subscriptionRepo;
            this.httpContextAccessor = httpContextAccessor;

        }
        public int TenantId => int.Parse(httpContextAccessor.HttpContext.User.FindFirst("TenantId").Value);
        public int UserId => int.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        public async Task<SubscriptionDetailsDTO> SubscriptionDetails()
        {
            var tenant = await _subscriptionRepo.GetTenantDetails(TenantId);
            return tenant.SubscriptionDetails();
        }
        public async Task<IReadOnlyList<SubscriptionPlanDTO?>> Subscriptionplans()
        {
            
            var repone = await _subscriptionRepo.GetSubscriptionPlans();
            return repone.Select(p => new SubscriptionPlanDTO
            {
                Id = p.Id,
                PlanName = p.PlanName,
                Price = p.Price,
                MonthlySampleLimit = p.MonthlySampleLimit
            }).ToList();

        }
        //public async Task<string> upgradePaln(SubscriptionPlanDTO subscriptionPlan)
        //{

        //}
    }
}
