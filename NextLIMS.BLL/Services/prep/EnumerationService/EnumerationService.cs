using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NextLIMS.BLL.DTO.prep;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.Repository;
using NextLIMS.DAL.Repository.SampleRepo.SampleWorkflowRepository;
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
        private readonly IGenericRepository<EnumerationDilution> _dilutionsrepository;
        private readonly ApplicationDbContext _context;
        private readonly ISampleWorkflowRepository _sampleWorkflowRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EnumerationService(IGenericRepository<EnumerationData> repository,
            IGenericRepository<EnumerationDilution> Dilutionsrepository,
            ApplicationDbContext context,
            ISampleWorkflowRepository sampleWorkflowRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _repo = repository;
            _dilutionsrepository = Dilutionsrepository;
            _context = context;
            _sampleWorkflowRepository = sampleWorkflowRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<int> SaveEnumerationPrepAsync(int sampleTestId, EnumerationPrepDto dto)
        {
            var createdBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var tenantId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("TenantId").Value);

            
            var sampleTest = await _context.SampleTests
                                            .Include(st => st.Sample)
                                            .FirstOrDefaultAsync(st => st.Id == sampleTestId);

            if (sampleTest == null) throw new Exception("Sample Test not found.");

            var enumerationData = new EnumerationData
            {
                SampleTestId = sampleTestId,
                TenantId = tenantId,
                Weight = dto.Weight,
                DiluentAmount = dto.DiluentAmount,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = createdBy
            };

            var enumerationDataAdded = await _repo.AddAsync(enumerationData);

            string? dilutionType = null;
            if (sampleTest.Sample != null && !string.IsNullOrEmpty(sampleTest.Sample.SampleType))
            {
                var cleanType = sampleTest.Sample.SampleType.Trim().ToLower();
                if (cleanType == "water") dilutionType = "Volume";
                else if (cleanType == "swab" || cleanType == "soil" || cleanType == "food") dilutionType = "dilution";
            }
            var enumerationilutionAdded = 0;
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
                enumerationilutionAdded =  await _dilutionsrepository.AddRangeAsync(dilutionEntities);
            }

            var workflowwadded = await _sampleWorkflowRepository.SetWorkflowToInProgressAsync(sampleTestId, tenantId, 1, "inprogress");

            if (enumerationDataAdded > 0 & enumerationilutionAdded > 0 && workflowwadded > 0) return 1;
            else return 0;
            
        }
    }
}