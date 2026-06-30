using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextLIMS.BLL.Services.Department;
using NextLIMS.BLL.DTO.DepartmentDto;
using System.Security.Claims;

namespace NexLIMS.API.Controllers.Department
{
    [ApiController]
    [Route("api/")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("departments")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var departments = await _departmentService.GetAllAsync(ct);

            if (!departments.Any())
                return NotFound();

            return Ok(departments);
        }

        [HttpPost("tenant/departments")]
        [Authorize]
        public async Task<IActionResult> SelectDepartment(
            [FromBody] SelectDepartmentRequestDto request,
            CancellationToken ct)
        {
            var tenantIdClaim = User.FindFirstValue("TenantId");
            if (tenantIdClaim == null)
               return Unauthorized();

            var tenantId = int.Parse(tenantIdClaim);

            try
            {
                var result = await _departmentService.SelectDepartmentAsync(tenantId, request, ct);
                return StatusCode(201, result);
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
    }
}
