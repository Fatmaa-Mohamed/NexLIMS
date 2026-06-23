using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Payment;
using NextLIMS.DAL.Repository.Subscription;
using System.Net.Http.Headers;

namespace NexLIMS.API.Controllers
{
    public class ActivateRequest
    {
        public string Email { get; set; } = string.Empty;
    }

    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly HttpClient _http;
        private readonly FawaterkSettings _settings;
        private readonly SubscriptionRepo _subscription;
        private readonly string _frontendUrl;
        private readonly ApplicationDbContext _context;

        public PaymentController(
            IHttpClientFactory httpClientFactory,
            SubscriptionRepo subscription,
            IOptions<FawaterkSettings> settings,
            IConfiguration configuration,
            ApplicationDbContext context)
        {
            _settings = settings.Value;
            _http = httpClientFactory.CreateClient();
            _http.BaseAddress = new Uri(_settings.BaseUrl);
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _settings.ApiKey);
            _http.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _subscription = subscription;
            _frontendUrl = configuration["App:FrontendUrl"] ?? "http://localhost:65050";
            _context = context;
        }

        // Step 1: get available payment methods
        [HttpGet("methods")]
        public async Task<IActionResult> GetPaymentMethods()
        {
            // Create the request message explicitly
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_settings.BaseUrl}/getPaymentmethods")
            {
                Content = new StringContent("", System.Text.Encoding.UTF8, "application/json")
            };

            var response = await _http.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, content);

            return Content(content, "application/json");
        }
        

        // Step 2: create invoice / initiate payment
        [HttpPost("create-invoice")]
        public async Task<IActionResult> CreateInvoice([FromBody] InvoiceRequest request)
        {

            var sub = await _subscription.GetSubscriptionPlan(request.SubscraptionId);
            if (request.PaymentMethodId <= 0)
                return BadRequest("PaymentMethodId is required. Call /api/payment/methods first.");

            var nameParts = request.CustomerName.Split(' ', 2);
            var firstName = nameParts[0];
            var lastName = nameParts.Length > 1 ? nameParts[1] : "Customer";
            var payload = new
            {
                payment_method_id = request.PaymentMethodId,
                cartTotal = sub?.Price.ToString("0.00") ?? "0.00",
                currency = "EGP",
                redirectOption = true,
                customer = new
                {
                    first_name = firstName,
                    last_name = lastName,
                    email = request.CustomerEmail,
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
                        name = sub.PlanName ?? "Default Plan",
                        price = sub.Price.ToString("0.00") ?? "0.00",
                        quantity = request.Quantity
                    }
                    }
            };

            var response = await _http.PostAsJsonAsync($"{_settings.BaseUrl}/invoiceInitPay", payload);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, content);

            return Content(content, "application/json");
        }

        [HttpPost("activate")]
        public async Task<IActionResult> Activate([FromBody] ActivateRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
                return BadRequest();

            var user = await _context.Users
                .Include(u => u.Tenant)
                .FirstOrDefaultAsync(u =>
                    u.Email == request.Email &&
                    u.Tenant != null &&
                    u.Tenant.SubscriptionStatus == "PendingPayment");

            if (user?.Tenant == null)
                return Ok(new { activated = false });

            user.Tenant.SubscriptionStatus = "Active";
            await _context.SaveChangesAsync();

            return Ok(new { activated = true });
        }
    }
}
