using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NextLIMS.BLL.DTO.prep;
using NextLIMS.DAL.Data;
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
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EnumerationService(IGenericRepository<EnumerationData> repository,ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repository;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<int> SaveEnumerationPrepAsync(int sampleTestId, EnumerationPrepDto dto)
        {
            var createdBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var tenantId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("TenantId").Value);

            var enumerationData = new EnumerationData
            {
                SampleTestId = sampleTestId,
                TenantId = tenantId,
                Weight = dto.Weight,
                DiluentAmount = dto.DiluentAmount,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = createdBy
            };

            await _repo.AddAsync(enumerationData);

            string? sampleType = await _context.SampleTests
                                                .Where(st => st.Id == sampleTestId)
                                                .Select(st => st.Sample.SampleType)
                                                .FirstOrDefaultAsync();

            string? dilutionType = null;
            if (!string.IsNullOrEmpty(sampleType))
            {
                var cleanType = sampleType.Trim().ToLower(); 
                if (cleanType == "water")
                    dilutionType = "Volume";
                else if (cleanType == "swab" || cleanType == "soil" || cleanType == "food")
                    dilutionType = "dilution";
            }

            if (dto.dilutions != null && dto.dilutions.Any())
            {
                var dilutionEntities = new List<EnumerationDilution>();

                foreach (var dilutionDto in dto.dilutions)
                {
                    dilutionEntities.Add(new EnumerationDilution
                    {
                        EnumerationDataId = enumerationData.Id, 
                        TenantId = tenantId,
                        DilutionOrVolume = dilutionDto.DilutionOrVolume,
                        VolumePlated = dilutionDto.VolumePlated,
                        DilutionType = dilutionType,
                        IsSelectedForCalculation = false,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = createdBy
                    });
                }

                
                await _context.EnumerationDilutions.AddRangeAsync(dilutionEntities);
                await _context.SaveChangesAsync();
            }

            return enumerationData.Id;
        }
    }
}