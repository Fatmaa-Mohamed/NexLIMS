namespace NextLIMS.DAL.Repository.Department
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Data.Models.Department>> GetAllAsync(CancellationToken ct = default);
        Task<Data.Models.Department?> GetByIdAsync(int departmentId, CancellationToken ct = default);
        Task<bool> TenantHasDepartmentAsync(int tenantId, int departmentId, CancellationToken ct = default);
        Task<Data.Models.TenantDepartment> AddTenantDepartmentAsync(Data.Models.TenantDepartment tenantDepartment, CancellationToken ct = default);
    }
}