using NextLIMS.DAL.Data.Models;

namespace NextLIMS.DAL.Repository.ClientPortal
{
    public interface IClientPortalRepository
    {
        Task<Sample?> GetInvitationContextAsync(
            int tenantId,
            int sampleId,
            CancellationToken cancellationToken = default);

        Task InvalidateUnusedOtpsAsync(
            int tenantId,
            int clientId,
            CancellationToken cancellationToken = default);

        Task AddOtpAsync(
            ClientOtpVerification otp,
            CancellationToken cancellationToken = default);

        Task<ClientOtpVerification?> GetOtpAsync(
            int verificationId,
            int tenantId,
            int clientId,
            CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default);
    }
}