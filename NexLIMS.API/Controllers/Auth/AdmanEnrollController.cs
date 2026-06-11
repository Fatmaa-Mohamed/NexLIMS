using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NexLIMS.BLL.DTO;
using NextLIMS.BLL.Services.SignupService;

namespace NexLIMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdmanEnrollController : ControllerBase
    {
        private readonly ISignupService _signupService;

        public AdmanEnrollController(ISignupService signupService)
        {
            _signupService = signupService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] RegisterDto request)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(request.Email))
            {
                return BadRequest();
            }

            var result = await _signupService.SignupAsync(request);

            if (result)
            {
                return Ok("Scussefull added");
            }

            return StatusCode(500);
        }
    }
}