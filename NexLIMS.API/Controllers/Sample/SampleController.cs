using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NexLIMS.API.Controllers.Sample
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        [HttpGet]
        public Task<IActionResult> GetAllSamples()
        {
            return Task.FromResult<IActionResult>(Ok("Hello World"));
        }
        [HttpGet("{id}")]
        public Task<IActionResult> GetSampleById()
        {
            return Task.FromResult<IActionResult>(Ok("Hello World"));
        }
        [HttpPost]
        public Task<IActionResult> CreateSample()
        {
            return Task.FromResult<IActionResult>(Ok("Hello World"));
        }
        [HttpPatch("{id}")]
        //put all dtos
        public Task<IActionResult> UpdateSample()
        {
            return Task.FromResult<IActionResult>(Ok("Hello World"));
        }


    }
}
