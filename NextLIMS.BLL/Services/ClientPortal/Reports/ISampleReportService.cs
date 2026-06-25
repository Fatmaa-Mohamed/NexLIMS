using NextLIMS.BLL.DTO.ClientPortal.Reports;

namespace NextLIMS.BLL.Services.ClientPortal.Reports
{
    public interface ISampleReportService 
    { 
        Task<GeneratedPdfReportDto> GenerateClientSampleReportAsync(
            string tenantSlug, 
            int tenantId, 
            int clientId, 
            int sampleId, 
            CancellationToken cancellationToken = default); 
    }
}
