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

        public async Task<List<Data.Models.TenantTest>> AddTenantTestsAsync(List<Data.Models.TenantTest> tenantTests, CancellationToken ct = default)
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

        public async Task<Data.Models.TenantTest?> GetTenantTestByIdAsync(int tenantTestId, int tenantId, CancellationToken ct = default)
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

        public async Task<(List<TenantTest> Items, int TotalCount)> GetAdminTenantTestsAsync(
            int tenantId,
            int? departmentId,
            string? testType,
            int? sampleTypeId,
            bool? isActive,
            int page,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _context.TenantTests
                .AsNoTracking()
                .Where(tt => tt.TenantId == tenantId)
                .Include(tt => tt.Test)
                    .ThenInclude(t => t.Department)
                .Include(tt => tt.TenantTestSampleTypes)
                    .ThenInclude(tts => tts.SampleType)
                .Include(tt => tt.Test)
                    .ThenInclude(t => t.ConfirmationTestTemplates
                        .Where(c => c.TenantId == null || c.TenantId == tenantId))
                .AsQueryable();

            if (departmentId.HasValue)
                query = query.Where(tt => tt.Test.DepartmentId == departmentId.Value);

            if (!string.IsNullOrEmpty(testType))
                query = query.Where(tt => tt.Test.TestType.ToLower() == testType.ToLower());

            if (sampleTypeId.HasValue)
                query = query.Where(tt => tt.TenantTestSampleTypes
                    .Any(tts => tts.SampleTypeId == sampleTypeId.Value));

            if (isActive.HasValue)
                query = query.Where(tt => tt.IsActive == isActive.Value);

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderBy(tt => tt.Test.TestName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, totalCount);
        }

        public async Task<(List<TenantTest> Items, int TotalCount)> GetPublicTenantTestsAsync(
            int tenantId,
            int? departmentId,
            string? testType,
            int? sampleTypeId,
            int page,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _context.TenantTests
                .AsNoTracking()
                .Where(tt => tt.TenantId == tenantId && tt.IsActive)
                .Include(tt => tt.Test)
                    .ThenInclude(t => t.Department)
                .Include(tt => tt.TenantTestSampleTypes)
                    .ThenInclude(tts => tts.SampleType)
                .AsQueryable();

            if (departmentId.HasValue)
                query = query.Where(tt => tt.Test.DepartmentId == departmentId.Value);

            if (!string.IsNullOrEmpty(testType))
                query = query.Where(tt => tt.Test.TestType.ToLower() == testType.ToLower());

            if (sampleTypeId.HasValue)
                query = query.Where(tt => tt.TenantTestSampleTypes
                    .Any(tts => tts.SampleTypeId == sampleTypeId.Value));

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderBy(tt => tt.Test.TestName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, totalCount);
        }
    }
}