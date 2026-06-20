using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NexLIMS.BLL.DTO;
using NextLIMS.BLL.Services.SignupService;
using NextLIMS.DAL.Data.Payment;

namespace NexLIMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdmanEnrollController : ControllerBase
    {
        private readonly ISignupService _signupService;
        private readonly IHttpClientFactory _httpClientFactory;

        public AdmanEnrollController(ISignupService signupService, IHttpClientFactory httpClientFactory)
        {
            _signupService = signupService;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> Signup([FromBody] RegisterDto request)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(request.Email))
            {
                return BadRequest();
            }

            var result = await _signupService.SignupAsync(request);

            if (!result)
                return StatusCode(500);

            return Ok(new { message = "Signup successful" });
        }
    }
}