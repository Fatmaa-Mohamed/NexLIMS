using NextLIMS.BLL.DTO.TestDto;

namespace NextLIMS.BLL.Services.Tests
{
    public interface ITestService
    {
        Task<IEnumerable<GetTestDto>> GetAllGlobalAsync(int? departmentId, string? testType, CancellationToken ct = default);
        Task<List<SelectTenantTestResponseDto>> SelectTenantTestsAsync(int tenantId, int createdBy, List<SelectTenantTestRequestDto> request, CancellationToken ct = default);
        Task<List<CreateCustomTestResponseDto>> CreateCustomTestsAsync(int tenantId, int createdBy, List<CreateCustomTestRequestDto> request, CancellationToken ct = default);
        Task<EditTenantTestResponseDto> EditTenantTestAsync(int tenantTestId, int tenantId, EditTenantTestRequestDto request, CancellationToken ct = default);
        Task<ToggleTenantTestStatusResponseDto> ToggleTenantTestStatusAsync(int tenantTestId, int tenantId, ToggleTenantTestStatusRequestDto request, CancellationToken ct = default);
    }
}