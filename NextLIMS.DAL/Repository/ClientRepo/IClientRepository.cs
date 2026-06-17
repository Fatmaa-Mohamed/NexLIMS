using NextLIMS.DAL.Data.Models;

namespace NextLIMS.DAL.Repository.ClientRepo
{
    public interface IClientRepository
    {
        Task<Client?> GetByNationalIdAndTenantAsync(
            int tenantId,
            string nationalId,
            CancellationToken cancellationToken = default);

        Task<Client?> GetByIdAndTenantAsync(
            int clientId,
            int tenantId,
            CancellationToken cancellationToken = default);

        Task<List<ClientSampleStatusCount>>
            GetSampleStatusCountsAsync(
                int clientId,
                int tenantId,
                CancellationToken cancellationToken = default);
    }
}