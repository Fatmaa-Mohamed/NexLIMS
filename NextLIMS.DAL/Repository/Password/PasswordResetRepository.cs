using Microsoft.EntityFrameworkCore;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;

namespace NextLIMS.DAL.Repositories
{
    public class PasswordResetRepository 
    {
        private readonly ApplicationDbContext _db;

        public PasswordResetRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _db.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task RemoveUnusedTokensAsync(int userId)
        {
            var tokens = await _db.passwordResets
                .Where(t => t.UserId == userId && !t.IsUsed)
                .ToListAsync();

            _db.passwordResets.RemoveRange(tokens);
        }

        public async Task AddPasswordResetAsync(PasswordReset reset)
        {
            await _db.passwordResets.AddAsync(reset);
        }

        public async Task<PasswordReset?> GetPasswordResetWithUserAsync(string token)
        {
            return await _db.passwordResets
                .Include(t => t.user)
                .FirstOrDefaultAsync(t => t.Token == token);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}