using Microsoft.AspNetCore.Http;
using NextLIMS.BLL.DTO.prep;
using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.Services.prep.EnumerationService
{
    public class EnumerationService : IEnumerationService
    {
        private readonly IGenericRepository<EnumerationData> _repo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EnumerationService(IGenericRepository<EnumerationData> repository, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> SaveEnumerationPrepAsync(int sampleTestId, EnumerationPrepDto dto)
        {
            var createdBy = int.Parse(_httpContextAccessor.HttpContext.User .FindFirst(ClaimTypes.NameIdentifier).Value);
            var tenantId = int.Parse(_httpContextAccessor.HttpContext.User .FindFirst("TenantId").Value);
            
            var enumerationData = new EnumerationData
            {
                SampleTestId = sampleTestId,
                TenantId = tenantId,
                Weight = dto.Weight,
                DiluentAmount = dto.DiluentAmount,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = createdBy,
            };
            return await _repo.AddAsync(enumerationData); ;
        }
    }
}
