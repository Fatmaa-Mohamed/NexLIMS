using Microsoft.EntityFrameworkCore;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;

namespace NextLIMS.DAL.Repositories
{
    public class EmployeeRepository 
    {
        private readonly ApplicationDbContext _db;

        public EmployeeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _db.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<PasswordReset?> GetPasswordResetWithUserAsync(string token)
        {
            return await _db.passwordResets
                .Include(x => x.user)
                .FirstOrDefaultAsync(x => x.Token == token);
        }

        public async Task<List<User>> GetEmployeesByTenantAsync(int tenantId)
        {
            return await _db.Users
                .Where(u => u.TenantId == tenantId)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}