using Microsoft.AspNetCore.Http;
using NextLIMS.BLL.DTO;
using NextLIMS.BLL.Extensions;
using NextLIMS.DAL.Repository.Subscription;
using NextLIMS.DAL.Repository.TeuntRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.Services.Tenant
{
    public class TenantSevices
    {
        private readonly TenantRepository _tenantRepository;



        private IHttpContextAccessor httpContextAccessor;

        public TenantSevices(TenantRepository tenantRepository, IHttpContextAccessor httpContextAccessor)
        {
            _tenantRepository = tenantRepository;
            this.httpContextAccessor = httpContextAccessor;

        }
        public int TenantId => int.Parse(httpContextAccessor.HttpContext.User.FindFirst("TenantId").Value);
        public int UserId => int.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        public async Task<TenantDTO?> GetTenant()
        {
            var reponse =  await _tenantRepository.GetTenant(TenantId);
            return reponse?.ToDTO();
        }
        public async Task<int> UpdateTenant(UpdateTenantDTO updateTenant)
        {
            var reponse = await _tenantRepository.UpdateTenantAsync(TenantId, updateTenant.name, updateTenant.address);
            return reponse;
        }

    }
}
