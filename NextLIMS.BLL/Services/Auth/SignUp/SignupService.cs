
using NexLIMS.BLL.DTO;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.Services.SignupService
{
    public class SignupService : ISignupService
    {
        private readonly ApplicationDbContext _context;

        public SignupService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SignupAsync(RegisterDto request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var tenant = new Tenant
                {
                    Name = request.TenantName,
                    Location = request.Location,
                    SubscriptionTier = request.SubscriptionTier,
                    SubscriptionStartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    SubscriptionEndDate = DateOnly.FromDateTime(DateTime.UtcNow).AddMonths(1),
                    SubscriptionStatus = "Active",
                    CreatedAt = DateTime.UtcNow,
                    MonthlySampleLimit = request.NumberofSampleInMonth

                };

                _context.Tenants.Add(tenant);
                await _context.SaveChangesAsync();

                var adminUser = new User
                {
                    TenantId = tenant.Id,
                    Name = request.AdminName,
                    Email = request.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    RoleId = 5 ,  // what is admain role Id
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(adminUser);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

      
    }
}