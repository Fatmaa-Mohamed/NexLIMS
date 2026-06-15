using Microsoft.AspNetCore.Http;
using NextLIMS.BLL.DTO.PrepDTOs;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.Services.prep.DetectionService
{
    public class DetectionService : IDetectionService
    {
        private readonly IGenericRepository<DetectionData> _repo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DetectionService(IGenericRepository<DetectionData> repository, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> SaveDetectionPrepAsync(int sampleTestId, DetectionPrepDto dto)
        {
            var createdBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var tenantId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("TenantId").Value);

            var detectionData = new DetectionData
            {
                SampleTestId = sampleTestId,
                TenantId = tenantId,
                Weight = dto.Weight,
                EnrichmentMedia = dto.EnrichmentMedia,
                MediaAmount = dto.MediaAmount,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = createdBy
            };
            return await _repo.AddAsync(detectionData);
        }
    }
}