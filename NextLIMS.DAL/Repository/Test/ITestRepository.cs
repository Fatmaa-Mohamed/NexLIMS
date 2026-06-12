using NextLIMS.DAL.Data.Models;

namespace NextLIMS.DAL.Repository.Test
{
    public interface ITestRepository
    {
        Task<IEnumerable<Data.Models.Test>> GetAllGlobalAsync(int? departmentId, string? testType, CancellationToken ct = default);
        Task<Data.Models.Test?> GetGlobalByIdAsync(int testId, CancellationToken ct = default);
        Task<bool> TenantTestExistsAsync(int tenantId, int testId, CancellationToken ct = default);
        Task<List<int>> GetDuplicateTenantTestIdsAsync(int tenantId, List<int> testIds, CancellationToken ct = default);
        Task<bool> SampleTypeExistsAsync(int sampleTypeId, CancellationToken ct = default);
        Task<bool> TenantDepartmentExistsAsync(int tenantId, int departmentId, CancellationToken ct = default);
        Task<List<Data.Models.TenantTest>> AddTenantTestsAsync(List<Data.Models.TenantTest> tenantTests, CancellationToken ct = default);
        Task<Data.Models.Test> AddTestAsync(Data.Models.Test test, CancellationToken ct = default);
        Task<SampleType?> GetSampleTypeByIdAsync(int sampleTypeId, CancellationToken ct = default);

        // Edit & Status
        Task<Data.Models.TenantTest?> GetTenantTestByIdAsync(int tenantTestId, int tenantId, CancellationToken ct = default);
        Task<bool> HasActiveSamplesAsync(int tenantTestId, CancellationToken ct = default);
        Task<List<SampleType>> GetSampleTypesByIdsAsync(List<int> sampleTypeIds, CancellationToken ct = default);

        // View Tenant tests
        Task<(List<TenantTest> Items, int TotalCount)> GetAdminTenantTestsAsync(
            int tenantId,
            int? departmentId,
            string? testType,
            int? sampleTypeId,
            bool? isActive,
            int page,
            int pageSize,
            CancellationToken ct = default);

        Task<(List<TenantTest> Items, int TotalCount)> GetPublicTenantTestsAsync(
            int tenantId,
            int? departmentId,
            string? testType,
            int? sampleTypeId,
            int page,
            int pageSize,
            CancellationToken ct = default);
    }
}