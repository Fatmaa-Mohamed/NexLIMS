using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextLIMS.BLL.Services.ClientPortal.Reports;

namespace NexLIMS.API.Controllers.ClientPortal 
{ 
    [ApiController]
    [Authorize(Policy = "ClientOnly")]
    [Route("api/client-portal/{slug}")] 
    public sealed class ClientSampleReportController : ControllerBase 
    {
        private readonly ISampleReportService _sampleReportService; 
        public ClientSampleReportController(ISampleReportService sampleReportService) 
        {
            _sampleReportService = sampleReportService; 
        } 

        [HttpGet("samples/{sampleId:int}/report")] 
        public async Task<IActionResult> DownloadSampleReport(
            [FromRoute] string slug, 
            [FromRoute] int sampleId, 
            CancellationToken cancellationToken) 
        { 
            if (!TryGetClientClaims(
                out var tenantId, 
                out var clientId)) 
            {
                return Unauthorized(new 
                {
                    message = "The client portal authentication token is invalid." 
                }); 
            } 
            try {
                var report = await _sampleReportService
                    .GenerateClientSampleReportAsync(
                        slug, 
                        tenantId, 
                        clientId, 
                        sampleId, 
                        cancellationToken); 
                return 
                    File(
                        report.Content, 
                        report.ContentType, 
                        report.FileName); 
            } 
            catch (KeyNotFoundException exception) 
            { 
                return NotFound(new 
                { 
                    message = exception.Message 
                }); 
            } 
            catch (InvalidOperationException exception) 
            {
                return Conflict(new 
                {
                    message = exception.Message 
                }); 
            } 
        } 
        private bool TryGetClientClaims(
            out int tenantId, 
            out int clientId) 
        {
            tenantId = 0; 
            clientId = 0; 
            var tenantIdValue = User.FindFirst("TenantId")?.Value; 
            var clientIdValue = User.FindFirst("ClientId")?.Value; 
            return int.TryParse(tenantIdValue, out tenantId) && int.TryParse(clientIdValue, out clientId); 
        } 
    } 
}