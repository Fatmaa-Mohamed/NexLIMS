using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NextLIMS.BLL.DTO.PrepDTOs;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.Repository;
using System.Security.Claims;

namespace NextLIMS.BLL.Services.prep.DetectionService
{
    public class DetectionService : IDetectionService
    {
        private readonly IGenericRepository<DetectionData> _repo;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DetectionService(IGenericRepository<DetectionData> repository, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repository;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> SaveDetectionPrepAsync(int sampleTestId, DetectionPrepDto dto)
        {
            var createdBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var tenantId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("TenantId").Value);

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var sampleTest = await _context.SampleTests
                                               .Include(st => st.Sample)
                                               .FirstOrDefaultAsync(st => st.Id == sampleTestId);

                if (sampleTest == null) throw new Exception("Sample Test not found.");

                // Delete existing prep data for this test (e.g. after a retest)
                var existing = await _context.DetectionData
                    .FirstOrDefaultAsync(dd => dd.SampleTestId == sampleTestId);
                if (existing != null)
                {
                    _context.DetectionData.Remove(existing);
                    await _context.SaveChangesAsync();
                }

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

                await _repo.AddAsync(detectionData);

                var workflow = await _context.SampleWorkflows
                                             .FirstOrDefaultAsync(w => w.SampleId == sampleTest.SampleId && w.TenantId == tenantId);
                if (workflow != null)
                {
                    workflow.Action = "inprogress";
                    workflow.StartDate = workflow.EndDate;
                    _context.SampleWorkflows.Update(workflow);
                }

                if (sampleTest.Sample != null)
                    sampleTest.Sample.Status = "InProgress";

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return detectionData.Id;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
