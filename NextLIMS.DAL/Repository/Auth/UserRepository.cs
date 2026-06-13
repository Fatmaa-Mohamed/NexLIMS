using Microsoft.EntityFrameworkCore;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;

namespace NextLIMS.DAL.Repositories
{
    public class UserRepository 
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAndTenantAsync(
            string email,
            int tenantId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Email == email &&
                    u.TenantId == tenantId);
        }
    }
}