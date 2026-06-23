using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NexLIMS.BLL.DTO;
using NextLIMS.BLL.Services.SignupService;
using NextLIMS.DAL.Data.Payment;
using NextLIMS.DAL.Repository.Subscription;
using System.Net.Http.Json;

namespace NexLIMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdmanEnrollController : ControllerBase
    {
        private readonly ISignupService _signupService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly SubscriptionRepo _repo;

        public AdmanEnrollController(ISignupService signupService, IHttpClientFactory httpClientFactory, SubscriptionRepo repo)
        {
            _signupService = signupService;
            _httpClientFactory = httpClientFactory;
            _repo = repo;
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

            try
            {
                var payload = new InvoiceRequest
                {
                    PaymentMethodId = request.PaymentMethodId,
                    CustomerName = request.TenantName,
                    SubscraptionId = request.SubscraptionID,
                    CustomerEmail = request.Email,
                };

                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsJsonAsync(
                    "https://localhost:7294/api/payment/create-invoice",
                    payload);

                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, content);

                return Content(content, "application/json");
            }
            catch
            {
                return Ok();
            }
        }
    }
}