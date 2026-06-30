using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NexLIMS.BLL.DTO;
using NextLIMS.BLL.Services.SignupService;
using NextLIMS.DAL.Data.Payment;
using NextLIMS.DAL.Repository.Subscription;
using System.Net.Http.Headers;
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
        private readonly FawaterkSettings _settings;
        private readonly string _frontendUrl;

        public AdmanEnrollController(
            ISignupService signupService,
            IHttpClientFactory httpClientFactory,
            SubscriptionRepo repo,
            IOptions<FawaterkSettings> settings,
            IConfiguration configuration)
        {
            _signupService = signupService;
            _httpClientFactory = httpClientFactory;
            _repo = repo;
            _settings = settings.Value;
            _frontendUrl = configuration["App:FrontendUrl"] ?? "http://localhost:65050";
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> Signup([FromBody] RegisterDto request)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(request.Email))
                return BadRequest();

            var sub = await _repo.GetSubscriptionPlan(request.SubscraptionID);
            if (sub == null)
                return BadRequest("Invalid subscription plan.");

            var result = await _signupService.SignupAsync(request);
            if (!result)
                return StatusCode(500, "Account creation failed.");

            var nameParts = request.AdminName.Split(' ', 2);
            var firstName = nameParts[0];
            var lastName = nameParts.Length > 1 ? nameParts[1] : "User";

            var payload = new
            {
                payment_method_id = request.PaymentMethodId,
                cartTotal = sub.Price.ToString("0.00"),
                currency = "EGP",
                redirectOption = true,
                customer = new
                {
                    first_name = firstName,
                    last_name = lastName,
                    email = request.Email,
                },
                redirectionUrls = new
                {
                    successUrl = $"{_frontendUrl}/payment/success",
                    failUrl    = $"{_frontendUrl}/payment/failed",
                    pendingUrl = $"{_frontendUrl}/payment/pending"
                },
                cartItems = new[]
                {
                    new
                    {
                        name = sub.PlanName ?? "Lab Plan",
                        price = sub.Price.ToString("0.00"),
                        quantity = 1
                    }
                }
            };

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _settings.ApiKey);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.PostAsJsonAsync($"{_settings.BaseUrl}/invoiceInitPay", payload);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, content);

            return Content(content, "application/json");
        }
    }
}