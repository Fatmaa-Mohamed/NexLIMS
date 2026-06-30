using Microsoft.AspNetCore.Http;
using NextLIMS.DAL.Repository.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.Services.Subscription
{
    public class SubscriptionUpdateServices
    {
        private IHttpContextAccessor httpContextAccessor;
        private readonly SubscriptionUpdateRepo _repo;

        public SubscriptionUpdateServices(SubscriptionUpdateRepo repo, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            this.httpContextAccessor = httpContextAccessor;
        }
        public int TenantId => int.Parse(httpContextAccessor.HttpContext.User.FindFirst("TenantId").Value);
        public async Task<string> AddQuota(int SampleNumber , bool isPaid)
        {
            if (isPaid)
            {
                var res = await _repo.AddQuota(TenantId, SampleNumber);
                if (res > 1) return "Sapmle Quota Added ";
            }
            return "Bad Request";
        }
        public async Task<string> UpgradeSubscription(int SampleNumber,int SubscriptionId, bool isPaid)
        {
            if (isPaid)
            {
                var res = await _repo.UpgrdeSubscription(TenantId, SubscriptionId);
                if (res > 1) return "Upgrde Subscription Done ";
            }
            return "Bad Request";
        }
    }
}
