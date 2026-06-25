using Microsoft.EntityFrameworkCore;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;

namespace NextLIMS.DAL.Repository.TenantRepo
{
    public class ClientTenantRepository : IClientTenantRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientTenantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Tenant?> GetActiveBySlugAsync(
            string slug,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return null;

            var normalizedSlug = slug.Trim().ToLower();

            return await _context.Tenants
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    tenant =>
                        tenant.Slug != null &&
                        tenant.Slug.Trim().ToLower() ==
                            normalizedSlug &&
                        tenant.IsActive == true,
                    cancellationToken);
        }
    }
}