using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextLIMS.BLL.DTO.TestDto;
using NextLIMS.BLL.Services.Tests;
using System.Security.Claims;

namespace NextLIMS.API.Controllers.Test
{
    [ApiController]
    [Route("api/")]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpGet("tests")]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? departmentId,
            [FromQuery] string? testType,
            CancellationToken ct)
        {
            if (departmentId.HasValue && departmentId.Value <= 0)
                return BadRequest(new { message = "departmentId must be a positive integer." });

            try
            {
                var tests = await _testService.GetAllGlobalAsync(departmentId, testType, ct);

                if (!tests.Any())
                    return NotFound();

                return Ok(tests);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("tenant/tests")]
        [Authorize]
        public async Task<IActionResult> SelectTenantTests(
            [FromBody] List<SelectTenantTestRequestDto> request,
            CancellationToken ct)
        {
            var tenantIdClaim = User.FindFirstValue("TenantId");
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (tenantIdClaim == null || userIdClaim == null)
                return Unauthorized();

            var tenantId = int.Parse(tenantIdClaim);
            var userId = int.Parse(userIdClaim);

            try
            {
                var result = await _testService.SelectTenantTestsAsync(tenantId, userId, request, ct);
                return StatusCode(201, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPost("tests")]
        [Authorize]
        public async Task<IActionResult> CreateCustomTests(
            [FromBody] List<CreateCustomTestRequestDto> request,
            CancellationToken ct)
        {
            var tenantIdClaim = User.FindFirstValue("TenantId");
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (tenantIdClaim == null || userIdClaim == null)
                return Unauthorized();

            var tenantId = int.Parse(tenantIdClaim);
            var userId = int.Parse(userIdClaim);

            try
            {
                var result = await _testService.CreateCustomTestsAsync(tenantId, userId, request, ct);
                return StatusCode(201, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("tenant/tests/{tenantTestId}")]
        [Authorize]
        public async Task<IActionResult> EditTenantTest(
            int tenantTestId,
            [FromBody] EditTenantTestRequestDto request,
            CancellationToken ct)
        {
            if (tenantTestId <= 0)
                return BadRequest(new { message = "tenantTestId must be a positive integer." });

            var tenantIdClaim = User.FindFirstValue("TenantId");
            if (tenantIdClaim == null)
                return Unauthorized();

            var tenantId = int.Parse(tenantIdClaim);

            try
            {
                var result = await _testService.EditTenantTestAsync(tenantTestId, tenantId, request, ct);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("tenant/tests/{tenantTestId}/status")]
        [Authorize]
        public async Task<IActionResult> ToggleTenantTestStatus(
            int tenantTestId,
            [FromBody] ToggleTenantTestStatusRequestDto request,
            CancellationToken ct)
        {
            if (tenantTestId <= 0)
                return BadRequest(new { message = "tenantTestId must be a positive integer." });

            var tenantIdClaim = User.FindFirstValue("TenantId");
            if (tenantIdClaim == null)
                return Unauthorized();

            var tenantId = int.Parse(tenantIdClaim);

            try
            {
                var result = await _testService.ToggleTenantTestStatusAsync(tenantTestId, tenantId, request, ct);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}