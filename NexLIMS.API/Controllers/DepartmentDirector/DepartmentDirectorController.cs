using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextLIMS.BLL.DTO.reasons;
using NextLIMS.BLL.Services.DepartmentDiractor;

namespace NexLIMS.API.Controllers.DepartmentDirector
{
    [Route("api/[controller]")]
    [ApiController]
    //reason taken 
    public class DepartmentDirectorController : ControllerBase
    {
        private DepartmentDirectorService _departmentDirectorService;

        public DepartmentDirectorController(DepartmentDirectorService departmentDirectorService)
        {
            _departmentDirectorService = departmentDirectorService;
        }

        [HttpPost("assignToAnalyst")]
        [Authorize]
        public async Task<IActionResult> assignSampleToAnalyst(int sampleid,int analystId,[FromBody] Reason reason)
        {
            await _departmentDirectorService.AssignSampleToAnalystAsync(sampleid,analystId,reason.reason);
            return Ok("Sample Assigned Successully");
        }

        [HttpGet("showMySamples")]
        [Authorize]
        public async Task<IActionResult> myAssignedSamples()
        {
            var result = await _departmentDirectorService.getMySamples();
            return Ok(result);
        }

        [HttpGet("dashboared")]
        [Authorize]
        public async Task<IActionResult> showDashboared() {

            return Ok(await _departmentDirectorService.dashboard()); 
        }
        [HttpPost("sampleRetest")]
        [Authorize]
        public async Task<IActionResult>SampleRetest(int sampleid, [FromBody] Reason reason)
        {

          var result=  await _departmentDirectorService.applyUpdateAsync(sampleid,reason.reason);
            if (!result)
                return BadRequest("Not Found Sample");
            return Ok(new { message="sample status updated"});
        }

        [HttpPost("Approve")]
        [Authorize]
        public async Task<IActionResult> Approvel(int sampleid, [FromBody] Reason reason)
        {

            var result = await _departmentDirectorService.approveAsync(sampleid,reason.reason);
            if (!result)
                return BadRequest("Not Found Sample");
            return Ok(new { message = "sample status Approved" });
        }

        [HttpGet("pendingApproval")]
        [Authorize]
        public async Task<IActionResult> PendingApprovel()
        {

            var result = await _departmentDirectorService.pendingApproval();
            
            return Ok(result);
        }

    }
}
