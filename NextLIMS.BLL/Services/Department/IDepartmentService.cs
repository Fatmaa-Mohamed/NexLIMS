using NextLIMS.BLL.DTO.DepartmentDto;

namespace NextLIMS.BLL.Services.Department
{
    public interface IDepartmentService
    {
        Task<IEnumerable<GetDepartmentDto>> GetAllAsync(CancellationToken ct = default);
        Task<SelectDepartmentResponseDto> SelectDepartmentAsync(int tenantId, SelectDepartmentRequestDto request, CancellationToken ct = default);
    }
}