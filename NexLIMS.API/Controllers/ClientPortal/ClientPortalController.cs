using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexLIMS.API.Extensions;
using NextLIMS.BLL.Services.ClientService;
using NextLIMS.BLL.Services.Tests;

namespace NexLIMS.API.Controllers.ClientPortal
{
    [Route("api/client-portal/{slug}")]
    [ApiController]
    [Authorize(Policy = "ClientOnly")]
    public class ClientPortalController : ControllerBase
    {
        private readonly IClientPortalService
            _clientPortalService;

        private readonly ITestService _testService;

        public ClientPortalController(
            IClientPortalService clientPortalService,
            ITestService testService)
        {
            _clientPortalService = clientPortalService;
            _testService = testService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMe(
            [FromRoute] string slug,
            CancellationToken cancellationToken)
        {
            if (!DoesSlugMatchToken(slug))
                return Forbid();

            var tenantId =
                User.GetRequiredIntClaim("TenantId");

            var clientId =
                User.GetRequiredIntClaim("ClientId");

            var result =
                await _clientPortalService.GetMeAsync(
                    tenantId,
                    clientId,
                    cancellationToken);

            return Ok(result);
        }

        [HttpGet("samples")]
        public async Task<IActionResult> GetSamples(
            [FromRoute] string slug,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 30,
            CancellationToken cancellationToken = default)
        {
            if (!DoesSlugMatchToken(slug))
                return Forbid();

            if (page <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "page must be a positive integer."
                });
            }

            if (pageSize < 1 || pageSize > 100)
            {
                return BadRequest(new
                {
                    message =
                        "pageSize must be between 1 and 100."
                });
            }

            var tenantId =
                User.GetRequiredIntClaim("TenantId");

            var clientId =
                User.GetRequiredIntClaim("ClientId");

            var result =
                await _clientPortalService.GetSamplesAsync(
                    tenantId,
                    clientId,
                    page,
                    pageSize,
                    cancellationToken);

            return Ok(result);
        }

        [HttpGet("tests")]
        public async Task<IActionResult> GetTests(
            [FromRoute] string slug,
            [FromQuery] int? departmentId,
            [FromQuery] string? testType,
            [FromQuery] int? sampleTypeId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 30,
            CancellationToken cancellationToken = default)
        {
            if (!DoesSlugMatchToken(slug))
                return Forbid();

            if (departmentId.HasValue &&
                departmentId.Value <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "departmentId must be a positive integer."
                });
            }

            if (sampleTypeId.HasValue &&
                sampleTypeId.Value <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "sampleTypeId must be a positive integer."
                });
            }

            if (page <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "page must be a positive integer."
                });
            }

            if (pageSize < 1 || pageSize > 100)
            {
                return BadRequest(new
                {
                    message =
                        "pageSize must be between 1 and 100."
                });
            }

            var tenantId =
                User.GetRequiredIntClaim("TenantId");

            try
            {
                var result =
                    await _testService
                        .GetPublicTenantTestsAsync(
                            tenantId,
                            departmentId,
                            testType,
                            sampleTypeId,
                            page,
                            pageSize,
                            cancellationToken);

                return Ok(result);
            }
            catch (ArgumentException exception)
            {
                return BadRequest(new
                {
                    message = exception.Message
                });
            }
        }

        private bool DoesSlugMatchToken(string routeSlug)
        {
            var tokenSlug =
                User.FindFirst("TenantSlug")?.Value;

            return string.Equals(
                routeSlug,
                tokenSlug,
                StringComparison.OrdinalIgnoreCase);
        }
    }
}