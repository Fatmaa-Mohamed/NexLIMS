using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextLIMS.BLL.DTO.Sample;
using NextLIMS.BLL.Services.SampleServic;

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
        [HttpPost]
        public async Task<IActionResult> CreateSample(SampleDTO sample)
        {
            var result = await _service.CreateSampleAsync(sample);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSamplesAsync([FromQuery] int page, [FromQuery] int pageSize)
        {

            var result = await _service.getAllSamplesAsync(page, pageSize);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSampleWithItsTests(int id)
        {
            var result = await _service.getSampleWithTestsById(id);

            return Ok(result);
        }
        [HttpPost("attach/{id}")]
        public async Task attachTestsToSample([FromRoute] int id, [FromBody] List<int> TestsIds)
        {
            await _service.attachTestsToSample(id, TestsIds);
        }


        [HttpPost("detach/{id}")]
        public async Task detachTestsToSample([FromRoute] int id, [FromBody] List<int> TestsIds)
        {
            await _service.detachTestsfromSample(id, TestsIds);

        }
        [HttpGet("filter")]
        public async Task<IActionResult> FilterSamples(

      [FromQuery] int? sampleId,
      [FromQuery] int? clientId)
        {
            var result = await _service.filterSampleByoptions(sampleId, clientId);
            return Ok(result);
        }
        [HttpGet("status/filter")]
        public async Task<IActionResult> FilterByStatus([FromQuery] string status)
        {
            var result = await _service.filterSampleByStatus(status);
            return Ok(result);
        }
    }
}
