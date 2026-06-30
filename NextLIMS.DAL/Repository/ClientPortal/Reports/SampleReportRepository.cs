using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace NextLIMS.DAL.Repository.ClientPortal.Reports
{
    public sealed class SampleReportRepository : ISampleReportRepository
    {
        private readonly ApplicationDbContext _context; 
        public SampleReportRepository(ApplicationDbContext context) 
        { 
            _context = context; 
        }
        public async Task<Sample?> GetClientSampleReportDataAsync(
            int tenantId, 
            int clientId, 
            int sampleId, 
            CancellationToken cancellationToken = default)
        {
            return await _context.Samples.AsNoTracking().AsSplitQuery()
                .Where(sample =>
                    sample.Id == sampleId &&
                    sample.TenantId == tenantId &&
                    sample.ClientId == clientId)
                
                .Include(sample => sample.Tenant)
                .Include(sample => sample.Client)

                .Include(sample => sample.SampleTests) 
                    .ThenInclude(sampleTest => sampleTest.TenantTest) 
                    .ThenInclude(tenantTest => tenantTest.Test) 
                    .ThenInclude(test => test.Department)

                .Include(sample => sample.SampleTests) 
                    .ThenInclude(sampleTest => sampleTest.ApprovedByUser) 
                    .ThenInclude(user => user.Role)

                .Include(sample => sample.SampleTests)
                    .ThenInclude(sampleTest => 
                        sampleTest.DetectionData)

                .Include(sample => sample.SampleTests)
                    .ThenInclude(sampleTest => 
                        sampleTest.EnumerationData)

                .Include(sample => sample.SampleWorkflows)
                    .ThenInclude(workflow => 
                        workflow.AssignedTo)
                    .ThenInclude(user => user.Role)

                .SingleOrDefaultAsync( cancellationToken); 
        }

        public async Task<IReadOnlyList<User>>
            GetActiveTenantAdminsAsync(
                int tenantId, 
                CancellationToken cancellationToken = default) 
        {
            return await _context.Users
                .AsNoTracking()
                .Include(user => user.Role)
                .Where(user => 
                    user.TenantId == tenantId &&
                    user.IsActive && user.Role != null &&
                    EF.Functions.Like(user.Role.Name, "%Admin%"))
                .OrderBy(user => user.Name)
                .ToListAsync(cancellationToken); 
        }
    }
}