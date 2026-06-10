using NextLIMS.BLL.DTO.DepartmentDto;
using NextLIMS.DAL.Repository.Department;

namespace NextLIMS.BLL.Services.Department
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<GetDepartmentDto>> GetAllAsync(CancellationToken ct = default)
        {
            var departments = await _departmentRepository.GetAllAsync(ct);

            return departments.Select(d => new GetDepartmentDto
            {
                Id = d.Id,
                Name = d.Name
            });
        }

        public async Task<SelectDepartmentResponseDto> SelectDepartmentAsync(int tenantId, SelectDepartmentRequestDto request, CancellationToken ct = default)
        {
            var department = await _departmentRepository.GetByIdAsync(request.DepartmentId, ct);
            if (department == null)
                throw new KeyNotFoundException($"Department with id {request.DepartmentId} was not found.");

            var alreadyExists = await _departmentRepository.TenantHasDepartmentAsync(tenantId, request.DepartmentId, ct);
            if (alreadyExists)
                throw new InvalidOperationException($"Department with id {request.DepartmentId} is already selected for this lab.");

            var tenantDepartment = new DAL.Data.Models.TenantDepartment
            {
                TenantId = tenantId,
                DepartmentId = request.DepartmentId,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _departmentRepository.AddTenantDepartmentAsync(tenantDepartment, ct);

            return new SelectDepartmentResponseDto
            {
                TenantId = created.TenantId,
                DepartmentId = created.DepartmentId,
                DepartmentName = department.Name,
                CreatedAt = created.CreatedAt
            };
        }
    }
}