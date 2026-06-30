using NextLIMS.BLL.DTO.Client;
using NextLIMS.BLL.DTO.TestDto;

namespace NextLIMS.BLL.Services.ClientService
{
    public interface IClientPortalService
    {
        Task<ClientMeDto> GetMeAsync(
            int tenantId,
            int clientId,
            CancellationToken cancellationToken = default);

        Task<PagedResult<ClientSampleDto>>
            GetSamplesAsync(
                int tenantId,
                int clientId,
                int page,
                int pageSize,
                CancellationToken cancellationToken = default);
    }
}