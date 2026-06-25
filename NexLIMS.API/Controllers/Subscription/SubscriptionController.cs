using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NexLIMS.API.Middlewares;
using NextLIMS.BLL.Services.Subscription;
using NextLIMS.DAL.Data.Payment;
using System.Net.Http.Headers;

namespace NexLIMS.API.Controllers.Subscription
{
    public record AddQuotaRequest(int SampleNumber, bool IsPaid);
    public record UpgradeRequest(int SubscriptionId, bool IsPaid);
    public record InitiatePaymentRequest(string Action, int? SampleNumber, int? PlanId);

    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly SubscriptionService _subscriptionService;
        private readonly SubscriptionUpdateServices _updateService;
        private readonly SubscriptionPaymentService _paymentService;
        private readonly HttpClient _http;
        private readonly FawaterkSettings _fawaterakSettings;
        private readonly string _frontendUrl;

        public SubscriptionController(
            SubscriptionService subscriptionService,
            SubscriptionUpdateServices updateService,
            SubscriptionPaymentService paymentService,
            IHttpClientFactory httpClientFactory,
            IOptions<FawaterkSettings> fawaterakSettings,
            IConfiguration configuration)
        {
            _subscriptionService  = subscriptionService;
            _updateService        = updateService;
            _paymentService       = paymentService;
            _fawaterakSettings    = fawaterakSettings.Value;
            _frontendUrl          = configuration["App:FrontendUrl"] ?? "http://localhost:65050";
            _http                 = httpClientFactory.CreateClient();
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fawaterakSettings.ApiKey);
        }

        [Authorize]
        [HttpGet("current")]
        [CheckPermission("Subscription")]
        public async Task<IActionResult> getSubscriptionDetails()
        {
            var respone = await _subscriptionService.SubscriptionDetails();
            return Ok(respone);
        }

        [Authorize]
        [HttpGet("plans")]
        [CheckPermission("Subscription")]
        public async Task<IActionResult> getSubscriptionPlans()
        {
            var respone = await _subscriptionService.Subscriptionplans();
            return Ok(respone);
        }

        [HttpGet("public-plans")]
        public async Task<IActionResult> GetPublicPlans()
        {
            var plans = await _subscriptionService.Subscriptionplans();
            return Ok(plans);
        }

        [Authorize]
        [HttpPost("add-quota")]
        [CheckPermission("Subscription")]
        public async Task<IActionResult> AddQuota([FromBody] AddQuotaRequest req)
        {
            var result = await _updateService.AddQuota(req.SampleNumber, req.IsPaid);
            return Ok(new { message = result });
        }

        [Authorize]
        [HttpPost("upgrade")]
        [CheckPermission("Subscription")]
        public async Task<IActionResult> UpgradeSubscription([FromBody] UpgradeRequest req)
        {
            var result = await _updateService.UpgradeSubscription(0, req.SubscriptionId, req.IsPaid);
            return Ok(new { message = result });
        }

        // ── Payment-gated endpoints ──────────────────────────────────────────

        [Authorize]
        [HttpPost("initiate-payment")]
        [CheckPermission("Subscription")]
        public async Task<IActionResult> InitiatePayment([FromBody] InitiatePaymentRequest req)
        {
            try
            {
                var amount = await _paymentService.CalculateAmountAsync(
                    req.Action, req.SampleNumber, req.PlanId);

                string invoiceId;
                string? paymentUrl = null;

                if (_paymentService.IsDev)
                {
                    // Dev: generate a fake invoice ID, skip Fawaterak
                    invoiceId = $"DEV-{Guid.NewGuid():N}";
                }
                else
                {
                    // Production: create a real Fawaterak invoice
                    var (tenantName, adminEmail, adminName) = await _paymentService.GetTenantInfoAsync();
                    var nameParts = adminName.Split(' ', 2);

                    var itemName = req.Action switch
                    {
                        "add_quota" => "Sample Quota",
                        "renew"     => "Plan Renewal",
                        "upgrade"   => "Subscription Upgrade",
                        _           => "Subscription"
                    };

                    var payload = new
                    {
                        payment_method_id = 1,
                        cartTotal         = amount.ToString("0.00"),
                        currency          = "EGP",
                        redirectOption    = true,
                        customer = new
                        {
                            first_name = nameParts[0],
                            last_name  = nameParts.Length > 1 ? nameParts[1] : "Admin",
                            email      = adminEmail,
                        },
                        redirectionUrls = new
                        {
                            successUrl = $"{_frontendUrl}/dashboard?payment=success&invoiceId={{invoice_id}}",
                            failUrl    = $"{_frontendUrl}/dashboard?payment=failed",
                            pendingUrl = $"{_frontendUrl}/dashboard?payment=pending",
                        },
                        cartItems = new[]
                        {
                            new { name = itemName, price = amount.ToString("0.00"), quantity = 1 }
                        }
                    };

                    var response = await _http.PostAsJsonAsync(
                        $"{_fawaterakSettings.BaseUrl}/invoiceInitPay", payload);

                    if (!response.IsSuccessStatusCode)
                        return StatusCode((int)response.StatusCode,
                            new { message = "Payment provider error. Please try again." });

                    var fawResult = await response.Content
                        .ReadFromJsonAsync<FawaterakCreateResponse>();

                    invoiceId  = fawResult?.Data?.InvoiceId ?? Guid.NewGuid().ToString();
                    paymentUrl = fawResult?.Data?.Url;
                }

                var result = await _paymentService.CreatePendingIntentAsync(
                    req.Action, req.SampleNumber, req.PlanId, amount, invoiceId, paymentUrl);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("confirm-payment")]
        [CheckPermission("Subscription")]
        public async Task<IActionResult> ConfirmPayment([FromQuery] string invoiceId)
        {
            if (string.IsNullOrWhiteSpace(invoiceId))
                return BadRequest(new { message = "invoiceId is required." });

            var result = await _paymentService.ExecuteAsync(invoiceId);
            return Ok(new { message = result });
        }
    }

    // Minimal Fawaterak response shape
    file class FawaterakCreateResponse
    {
        public FawaterakCreateData? Data { get; set; }
    }
    file class FawaterakCreateData
    {
        [System.Text.Json.Serialization.JsonPropertyName("invoice_id")]
        public string InvoiceId { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }
}
