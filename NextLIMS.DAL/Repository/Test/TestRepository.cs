using Microsoft.EntityFrameworkCore;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;

namespace NextLIMS.DAL.Repository.Test
{
    public class TestRepository : ITestRepository
    {
        private readonly ApplicationDbContext _context;

        public TestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Data.Models.Test>> GetAllGlobalAsync(int? departmentId, string? testType, CancellationToken ct = default)
        {
            var query = _context.Tests
                .AsNoTracking()
                .Where(t => t.TenantId == null)
                .Include(t => t.TestSampleTypes)
                    .ThenInclude(ts => ts.SampleType)
                .Include(t => t.ConfirmationTestTemplates
                    .Where(c => c.TenantId == null))
                .AsQueryable();

            if (departmentId.HasValue)
                query = query.Where(t => t.DepartmentId == departmentId.Value);

            if (!string.IsNullOrEmpty(testType))
                query = query.Where(t => t.TestType.ToLower() == testType.ToLower());

            return await query.ToListAsync(ct);
        }

        public async Task<Data.Models.Test?> GetGlobalByIdAsync(int testId, CancellationToken ct = default)
        {
            return await _context.Tests
                .AsNoTracking()
                .Include(t => t.TestSampleTypes)
                    .ThenInclude(ts => ts.SampleType)
                .FirstOrDefaultAsync(t => t.Id == testId && t.TenantId == null, ct);
        }

        public async Task<bool> TenantTestExistsAsync(int tenantId, int testId, CancellationToken ct = default)
        {
            return await _context.TenantTests
                .AnyAsync(tt => tt.TenantId == tenantId && tt.TestId == testId, ct);
        }

        public async Task<List<int>> GetDuplicateTenantTestIdsAsync(int tenantId, List<int> testIds, CancellationToken ct = default)
        {
            return await _context.TenantTests
                .Where(tt => tt.TenantId == tenantId && testIds.Contains(tt.TestId))
                .Select(tt => tt.TestId)
                .ToListAsync(ct);
        }

        public async Task<bool> SampleTypeExistsAsync(int sampleTypeId, CancellationToken ct = default)
        {
            return await _context.SampleTypes
                .AnyAsync(st => st.Id == sampleTypeId, ct);
        }

        public async Task<SampleType?> GetSampleTypeByIdAsync(int sampleTypeId, CancellationToken ct = default)
        {
            return await _context.SampleTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(st => st.Id == sampleTypeId, ct);
        }

        public async Task<bool> TenantDepartmentExistsAsync(int tenantId, int departmentId, CancellationToken ct = default)
        {
            return await _context.TenantDepartments
                .AnyAsync(td => td.TenantId == tenantId && td.DepartmentId == departmentId, ct);
        }

        public async Task<List<TenantTest>> AddTenantTestsAsync(List<TenantTest> tenantTests, CancellationToken ct = default)
        {
            await _context.TenantTests.AddRangeAsync(tenantTests, ct);
            await _context.SaveChangesAsync(ct);
            return tenantTests;
        }

        public async Task<Data.Models.Test> AddTestAsync(Data.Models.Test test, CancellationToken ct = default)
        {
            await _context.Tests.AddAsync(test, ct);
            await _context.SaveChangesAsync(ct);
            return test;
        }

        public async Task<TenantTest?> GetTenantTestByIdAsync(int tenantTestId, int tenantId, CancellationToken ct = default)
        {
            return await _context.TenantTests
                .Include(tt => tt.Test)
                .Include(tt => tt.TenantTestSampleTypes)
                    .ThenInclude(tts => tts.SampleType)
                .FirstOrDefaultAsync(tt => tt.Id == tenantTestId && tt.TenantId == tenantId, ct);
        }

        public async Task<bool> HasActiveSamplesAsync(int tenantTestId, CancellationToken ct = default)
        {
            var inactiveStatuses = new[] { "Approved", "ReportGenerated" };

            return await _context.SampleTests
                .AnyAsync(st =>
                    st.TenantTestId == tenantTestId &&
                    !inactiveStatuses.Contains(st.Status), ct);
        }

        public async Task<List<SampleType>> GetSampleTypesByIdsAsync(List<int> sampleTypeIds, CancellationToken ct = default)
        {
            return await _context.SampleTypes
                .AsNoTracking()
                .Where(st => sampleTypeIds.Contains(st.Id))
                .ToListAsync(ct);
        }
    }
}