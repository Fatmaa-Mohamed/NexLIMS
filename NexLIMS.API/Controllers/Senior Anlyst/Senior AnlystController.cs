using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.Repository.DepartmentDirector;

namespace NexLIMS.API.Controllers.Senior_Anlyst
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SeniorAnlystController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public SeniorAnlystController(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        public int TenantId => int.Parse(httpContextAccessor.HttpContext.User.FindFirst("TenantId").Value);
        [HttpGet("GetSeniorAnlyst")]
        public async Task<IActionResult> GetSeniorAnlyst()
        {
            var res = await _dbContext.Users
                .Where(u => u.TenantId == TenantId &&
                            u.Role.RolePermissions.Any(rp => rp.Permission.Name == "Get_Sample_Details_With_Preps") &&
                            !u.Role.RolePermissions.Any(rp => rp.Permission.Name == "CreateRole"))
                .Select(u => new { u.Id, u.Name })
                .ToListAsync();

            return Ok(res);
        }


        [HttpGet("AssignToSeniorAnalyst")]
        public async Task<IActionResult> AssignToSeniorAnalyst(int sampleid, int analystId)
        {
            var sampleTests = await _dbContext.SampleTests
                .Where(x => x.SampleId == sampleid && x.Sample.TenantId == TenantId)
                .ToListAsync();

            foreach (var sampleTest in sampleTests)
            {
                sampleTest.AssignedToUserId = analystId;
            }

            await _dbContext.SampleWorkflows.AddAsync(new SampleWorkflow
            {
                TenantId     = TenantId,
                SampleId     = sampleid,
                AssignedToId = analystId,
                Level        = 2,
                Action       = "InProgress",
                StartDate    = DateTime.UtcNow,
                Flag         = true,
            });

            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("submitForApproval")]
        public async Task<IActionResult> SubmitForApproval(int sampleId)
        {
            var sampleTests = await _dbContext.SampleTests
                .Where(st => st.SampleId == sampleId && st.Sample.TenantId == TenantId)
                .ToListAsync();

            if (!sampleTests.Any())
                return NotFound(new { error = "No tests found for this sample." });

            if (sampleTests.Any(st => st.Status != "Pending Approval"))
                return BadRequest(new { error = "All tests must be submitted before sending for approval." });

            var director = await _dbContext.Users
                .Where(u => u.TenantId == TenantId &&
                            u.Role.RolePermissions.Any(rp => rp.Permission.Name == "AssignToAnalystASpeciicSample"))
                .FirstOrDefaultAsync();

            if (director == null)
                return BadRequest(new { error = "No Department Director found for this tenant." });

            var latestWorkflow = await _dbContext.SampleWorkflows
                .OrderByDescending(w => w.Id)
                .FirstOrDefaultAsync(w => w.SampleId == sampleId && w.TenantId == TenantId);

            if (latestWorkflow != null)
            {
                latestWorkflow.EndDate = DateTime.UtcNow;
                _dbContext.SampleWorkflows.Update(latestWorkflow);
            }

            await _dbContext.SampleWorkflows.AddAsync(new SampleWorkflow
            {
                TenantId     = TenantId,
                SampleId     = sampleId,
                Level        = 3,
                Action       = "Pending Approval",
                AssignedToId = director.Id,
                StartDate    = DateTime.UtcNow,
                Flag         = true,
            });

            var sample = await _dbContext.Samples
                .FirstOrDefaultAsync(s => s.Id == sampleId && s.TenantId == TenantId);

            if (sample == null)
                return NotFound(new { error = "Sample not found." });

            sample.Status = "Pending Approval";

            await _dbContext.SaveChangesAsync();
            return Ok(new { message = "Sample submitted for approval." });
        }

    }
}
