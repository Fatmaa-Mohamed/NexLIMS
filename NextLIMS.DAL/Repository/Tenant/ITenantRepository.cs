using NextLIMS.DAL.Data.Models;

namespace NextLIMS.DAL.Repository.TenantRepo
{
    public interface ITenantRepository
    {
        Task<Tenant?> GetActiveBySlugAsync(
            string slug,
            CancellationToken cancellationToken = default);
    }
}