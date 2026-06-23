
using Microsoft.EntityFrameworkCore;
using NexLIMS.BLL.DTO;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.Repository.Subscription;
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
        private readonly SubscriptionRepo _subscription;

        public SignupService(ApplicationDbContext context , SubscriptionRepo subscription)
        {
            _context = context;
            _subscription = subscription;
        }

        private async Task<string> GenerateUniqueSlugAsync(string tenantName)
        {
            var baseSlug = SlugHelper.Generate(tenantName);
            var slug = baseSlug;
            var counter = 1;

            while (await _context.Tenants.AnyAsync(t => t.Slug == slug))
            {
                slug = $"{baseSlug}-{counter}";
                counter++;
            }

            return slug;
        }

        public async Task<bool> SignupAsync(RegisterDto request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            var sub = await _subscription.GetSubscriptionPlan(request.SubscraptionID);
            try
            {
                var tenant = new NextLIMS.DAL.Data.Models.Tenant
                {
                    Name = request.TenantName,
                    Location = request.Location,
                    SubscriptionTier = sub.PlanName,
                    SubscriptionStartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    SubscriptionEndDate = DateOnly.FromDateTime(DateTime.UtcNow).AddMonths(1),
                    SubscriptionStatus = "Active",
                    Slug = await GenerateUniqueSlugAsync(request.TenantName),
                    CreatedAt = DateTime.UtcNow,
                    MonthlySampleLimit = sub?.MonthlySampleLimit

                };

                _context.Tenants.Add(tenant);
                await _context.SaveChangesAsync();



                var adminUser = new User
                {
                    TenantId = tenant.Id,
                    Name = request.AdminName,
                    Email = request.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    RoleId = 1 ,  // what is admain role Id
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