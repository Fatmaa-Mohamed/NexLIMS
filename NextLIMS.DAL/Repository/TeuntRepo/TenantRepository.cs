using Microsoft.EntityFrameworkCore;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.DAL.Repository.TeuntRepo
{
    public class TenantRepository
    {
        private readonly ApplicationDbContext _context;

        public TenantRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task <int>UpdateTenantAsync(int tenantId, string newName, string newLocation)
        {
            var tenant = await _context.Tenants.FindAsync(tenantId);

            if (tenant != null)
            {
                tenant.Name = newName;
                tenant.Location = newLocation;
                return await _context.SaveChangesAsync();
            }
            return 0;
        }
        public async Task<Tenant?> GetTenant(int tenantId)
        {
            return await _context.Tenants.FirstOrDefaultAsync(t => t.Id == tenantId);
        }

    }
}

