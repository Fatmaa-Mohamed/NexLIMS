using Microsoft.EntityFrameworkCore;
using NextLIMS.DAL.Data;

namespace NextLIMS.DAL.Repository.Department
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Data.Models.Department>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Departments
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<Data.Models.Department?> GetByIdAsync(int departmentId, CancellationToken ct = default)
        {
            return await _context.Departments
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == departmentId, ct);
        }

        public async Task<bool> TenantHasDepartmentAsync(int tenantId, int departmentId, CancellationToken ct = default)
        {
            return await _context.TenantDepartments
                .AnyAsync(td => td.TenantId == tenantId && td.DepartmentId == departmentId, ct);
        }

        public async Task<Data.Models.TenantDepartment> AddTenantDepartmentAsync(Data.Models.TenantDepartment tenantDepartment, CancellationToken ct = default)
        {
            _context.TenantDepartments.Add(tenantDepartment);
            await _context.SaveChangesAsync(ct);
            return tenantDepartment;
        }
    }
}