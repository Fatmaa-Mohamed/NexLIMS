using Microsoft.EntityFrameworkCore;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;

namespace NextLIMS.DAL.Repositories
{
    public class RoleRepository 
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddRoleAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
        }

        public async Task<Role?> GetRoleByIdAndTenantAsync(
            int roleId,
            int tenantId)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r =>
                    r.Id == roleId &&
                    r.TenantId == tenantId);
        }

        public async Task<List<Role>> GetRolesByTenantAsync(
            int tenantId)
        {
            return await _context.Roles
                .Where(r => r.TenantId == tenantId)
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task<List<int>> GetExistingPermissionIdsAsync(
            ICollection<int> permissionIds)
        {
            return await _context.Permissions
                .Where(p => permissionIds.Contains(p.Id))
                .Select(p => p.Id)
                .ToListAsync();
        }

        public async Task<List<int>> GetAttachedPermissionIdsAsync(
            int roleId)
        {
            return await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.PermissionId)
                .ToListAsync();
        }

        public async Task AddRolePermissionsAsync(
            IEnumerable<RolePermission> rolePermissions)
        {
            await _context.RolePermissions
                .AddRangeAsync(rolePermissions);
        }

        public async Task<RolePermission?> GetRolePermissionAsync(
            int roleId,
            int permissionId)
        {
            return await _context.RolePermissions
                .FirstOrDefaultAsync(rp =>
                    rp.RoleId == roleId &&
                    rp.PermissionId == permissionId);
        }

        public void RemoveRolePermission(
            RolePermission rolePermission)
        {
            _context.RolePermissions.Remove(rolePermission);
        }
        public async Task<bool> RoleExistsForTenantAsync(int roleId, int tenantId)
        {
            return await _context.Roles
                .AnyAsync(r => r.Id == roleId && r.TenantId == tenantId);
        }

        public async Task<List<object>> GetRolePermissionsAsync(
            int roleId
            )
        {
            return await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => new
                {
                    rp.Permission.Id,
                    rp.Permission.Name
                })
                .Cast<object>()
                .ToListAsync();
        }

        //role permissions
        public async Task<List<RolePermission>> GetRolePermissionsAsync2(
            int roleId,
            ICollection<int> permissionIds
            )
        {
            return await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId && permissionIds.Contains(rp.PermissionId))
                .ToListAsync();
        }

        public void RemoveRolePermissionsRange(IEnumerable<RolePermission> rolePermissions)
        {
            _context.RolePermissions.RemoveRange(rolePermissions);
        }



        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}