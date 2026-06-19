using Microsoft.EntityFrameworkCore;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;

namespace NextLIMS.DAL.Repository.ClientRepo
{
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Client?> GetByNationalIdAndTenantAsync(
            int tenantId,
            string nationalId,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(nationalId))
                return null;

            var normalizedNationalId =
                NormalizeNationalId(nationalId);

            return await _context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    client =>
                        client.TenantId == tenantId &&
                        client.NID != null &&
                        client.NID
                            .Trim()
                            .Replace(" ", "")
                            .Replace("-", "") ==
                        normalizedNationalId,
                    cancellationToken);
        }

        public async Task<Client?> GetByIdAndTenantAsync(
            int clientId,
            int tenantId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    client =>
                        client.Id == clientId &&
                        client.TenantId == tenantId,
                    cancellationToken);
        }

        public async Task<List<ClientSampleStatusCount>>
            GetSampleStatusCountsAsync(
                int clientId,
                int tenantId,
                CancellationToken cancellationToken = default)
        {
            return await _context.Samples
                .AsNoTracking()
                .Where(sample =>
                    sample.ClientId == clientId &&
                    sample.TenantId == tenantId)
                .GroupBy(sample => sample.Status)
                .Select(group =>
                    new ClientSampleStatusCount
                    {
                        Status = group.Key,
                        Count = group.Count()
                    })
                .ToListAsync(cancellationToken);
        }

        private static string NormalizeNationalId(
            string nationalId)
        {
            return nationalId
                .Trim()
                .Replace(" ", "")
                .Replace("-", "");
        }
    }

    public class ClientSampleStatusCount
    {
        public string Status { get; set; } = string.Empty;

        public int Count { get; set; }
    }
}