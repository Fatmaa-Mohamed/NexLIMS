using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NexLIMS.API.Middlewares;
using NextLIMS.BLL.DTO.Sample;
using NextLIMS.BLL.Services.SampleServic;
using NextLIMS.DAL.Repository.SampleRepo;

namespace NexLIMS.API.Controllers.Sample
{

    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly SampleService _service;
        public SampleController(SampleService service)
        {
            this._service = service;
        }
        [Authorize]
        [HttpPost]
        [CheckPermission("CreateSample")]
        public async Task<IActionResult> CreateSample(SampleDTO sample)
        {
            var result = await _service.CreateSampleAsync(sample);
            return Ok(result);
        }
        [Authorize]
        [HttpGet]
        [CheckPermission("GetAllSamples")]
        public async Task<IActionResult> GetAllSamplesAsync([FromQuery] int page, [FromQuery] int pageSize)
        {

            var result = await _service.getAllSamplesAsync(page, pageSize);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("{id}")]
        [CheckPermission("GetSampleWithItsTests")]
        public async Task<IActionResult> GetSampleWithItsTests(int id)
        {
            var result = await _service.getSampleWithTestsById(id);

            return Ok(result);
        }
        [Authorize]
        [HttpPost("attach/{id}")]
        [CheckPermission("Add_Sample_To_Test")]
        public async Task attachTestsToSample([FromRoute] int id, [FromBody] List<int> TestsIds)
        {
            await _service.attachTestsToSample(id, TestsIds);
        }

        [Authorize]
        [HttpPost("detach/{id}")]
        [CheckPermission("Remove_Sample_From_Test")]
        public async Task detachTestsToSample([FromRoute] int id, [FromBody] List<int> TestsIds)
        {
            await _service.detachTestsfromSample(id, TestsIds);

        }
        [Authorize]
        [HttpGet("filter")]
        [CheckPermission("Filter_Samples_by_sampleId_OR_ClientId")]
        public async Task<IActionResult> FilterSamples(

      [FromQuery] int? sampleId,
      [FromQuery] int? clientId)
        {
            var result = await _service.filterSampleByoptions(sampleId, clientId);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("status/filter")]
        [CheckPermission("Filter_Samples_by_Status")]

        public async Task<IActionResult> FilterByStatus([FromQuery] string status)
        {
            var result = await _service.filterSampleByStatus(status);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("confirmation/{TestId}")]
        [CheckPermission("Get_Confirmation_Templates")]
        public async Task<IActionResult> GetConfirmationTemplates([FromRoute] int TestId)
        {
            var templates = await _service.GetConfirmationTemplatesByTestId(TestId);
            return Ok(new { confirmation = templates ?? new List<string>() });
        }
        [Authorize]
        [HttpGet("{SampleId}/Get-analyst-prep")]
        [CheckPermission("Get_Sample_Details_With_Preps")]
        public async Task<IActionResult> GetSampleDetailsWithPreps([FromRoute] int SampleId)
        {
            var templates = await _service.GetSampleDetailsWithPreps(SampleId);
            return Ok(templates);
            
        }



    }
}
