using Microsoft.EntityFrameworkCore;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;

namespace NextLIMS.DAL.Repositories
{
    public class PermissionRepository 
    {
        private readonly ApplicationDbContext _context;

        public PermissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Permission>> GetAllPermissionsAsync()
        {
            return await _context.Permissions.ToListAsync();
        }
    }
}