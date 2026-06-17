using Microsoft.EntityFrameworkCore;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;

namespace NextLIMS.DAL.Repository.ClientPortal
{
    public class ClientPortalRepository
        : IClientPortalRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientPortalRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Sample?> GetInvitationContextAsync(
            int tenantId,
            int sampleId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Samples
                .AsNoTracking()
                .Include(sample => sample.Client)
                .Include(sample => sample.Tenant)
                .FirstOrDefaultAsync(
                    sample =>
                        sample.Id == sampleId &&
                        sample.TenantId == tenantId,
                    cancellationToken);
        }

        public async Task InvalidateUnusedOtpsAsync(
            int tenantId,
            int clientId,
            CancellationToken cancellationToken = default)
        {
            var unusedOtps =
                await _context.ClientOtpVerifications
                    .Where(otp =>
                        otp.TenantId == tenantId &&
                        otp.ClientId == clientId &&
                        !otp.IsUsed)
                    .ToListAsync(cancellationToken);

            foreach (var otp in unusedOtps)
            {
                otp.IsUsed = true;
            }
        }

        public async Task<ClientOtpVerification?> GetOtpAsync(
            int verificationId,
            int tenantId,
            int clientId,
            CancellationToken cancellationToken = default)
        {
            return await _context.ClientOtpVerifications
                .FirstOrDefaultAsync(
                    otp =>
                        otp.Id == verificationId &&
                        otp.TenantId == tenantId &&
                        otp.ClientId == clientId,
                    cancellationToken);
        }
        public async Task AddOtpAsync(
            ClientOtpVerification otp,
            CancellationToken cancellationToken = default)
        {
            await _context.ClientOtpVerifications.AddAsync(
                otp,
                cancellationToken);
        }

        public Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(
                cancellationToken);
        }
    }
}