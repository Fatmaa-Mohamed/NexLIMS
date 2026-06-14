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

            var payload = new InvoiceRequest
            {
                PaymentMethodId = request.PaymentMethodId,
                CustomerName = request.TenantName,
                ProductName = request.SubscriptionTier,
                CustomerEmail = request.Email,
                Amount = request.Amount,
            };

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync(
                "https://localhost:7294/api/payment/create-invoice",
                payload);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, content);

            // Return the invoice info (including redirectTo URL) so frontend can redirect user to pay
            return Content(content, "application/json");
        }
    }
}