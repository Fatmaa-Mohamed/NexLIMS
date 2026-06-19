using NextLIMS.BLL.DTO.Client;
using NextLIMS.BLL.DTO.TestDto;
using NextLIMS.DAL.Repository.ClientRepo;
using NextLIMS.DAL.Repository.SampleRepo;

namespace NextLIMS.BLL.Services.ClientService
{
    public class ClientPortalService
    : IClientPortalService
    {
        private readonly IClientRepository
            _clientRepository;

        private readonly SampleRepository
            _sampleRepository;

        public ClientPortalService(
            IClientRepository clientRepository,
            SampleRepository sampleRepository)
        {
            _clientRepository = clientRepository;
            _sampleRepository = sampleRepository;
        }

        public async Task<ClientMeDto> GetMeAsync(
            int tenantId,
            int clientId,
            CancellationToken cancellationToken = default)
        {
            var client =
                await _clientRepository.GetByIdAndTenantAsync(
                    clientId,
                    tenantId,
                    cancellationToken);

            if (client == null)
                throw new KeyNotFoundException(
                    "Client was not found.");

            var statusCounts =
                await _clientRepository
                    .GetSampleStatusCountsAsync(
                        clientId,
                        tenantId,
                        cancellationToken);

            var normalizedStatusCounts = statusCounts
                .GroupBy(item =>
                    NormalizeStatus(item.Status))
                .Select(group =>
                    new ClientSampleStatusSummaryDto
                    {
                        Status = group.Key,
                        Count = group.Sum(item => item.Count)
                    })
                .OrderBy(item =>
                    GetStatusOrder(item.Status))
                .ToList();

            return new ClientMeDto
            {
                ClientId = client.Id,
                Name = client.Name,
                NationalId = client.NID,
                PhoneNumber = client.PhoneNumber,
                TotalSamples = normalizedStatusCounts
                    .Sum(item => item.Count),
                SampleStatuses = normalizedStatusCounts
            };
        }

        public async Task<PagedResult<ClientSampleDto>>
            GetSamplesAsync(
                int tenantId,
                int clientId,
                int page,
                int pageSize,
                CancellationToken cancellationToken = default)
        {
            var (samples, totalCount) =
                await _sampleRepository
                    .GetClientSamplesAsync(
                        tenantId,
                        clientId,
                        page,
                        pageSize,
                        cancellationToken);

            return new PagedResult<ClientSampleDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,

                TotalPages = (int)Math.Ceiling(
                    (double)totalCount / pageSize),

                Items = samples.Select(sample =>
                    new ClientSampleDto
                    {
                        SampleId = sample.Id,
                        SampleName = sample.SampleName,
                        SampleType = sample.SampleType,
                        Status = NormalizeStatus(sample.Status),
                        RegisteredAt = sample.CreatedAt,

                        CanDownloadReport =
                            string.Equals(
                                sample.Status,
                                "Approved",
                                StringComparison.OrdinalIgnoreCase)
                    })
                    .ToList()
            };
        }

        private static string NormalizeStatus(string? status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return "Unknown";

            return status.Trim().ToLowerInvariant() switch
            {
                "registered" => "Registered",
                "pending" => "Pending",
                "in progress" => "In Progress",
                "inprogress" => "In Progress",
                "pending approval" => "Pending Approval",
                "pendingapproval" => "Pending Approval",
                "approved" => "Approved",
                _ => status.Trim()
            };
        }

        private static int GetStatusOrder(string status)
        {
            return status switch
            {
                "Registered" => 1,
                "Pending" => 2,
                "In Progress" => 3,
                "Pending Approval" => 4,
                "Approved" => 5,
                _ => 100
            };
        }
    }
}