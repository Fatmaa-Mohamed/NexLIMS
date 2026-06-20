using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NextLIMS.DAL.Data.Payment;
using System.Net.Http.Headers;

namespace NexLIMS.API.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly HttpClient _http;
        private readonly FawaterkSettings _settings;

        public PaymentController(IHttpClientFactory httpClientFactory, IOptions<FawaterkSettings> settings)
        {
            _settings = settings.Value;
            _http = httpClientFactory.CreateClient();
            _http.BaseAddress = new Uri(_settings.BaseUrl);
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _settings.ApiKey);
            _http.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
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
            if (request.Amount <= 0)
                return BadRequest("Amount must be greater than zero.");

            if (request.PaymentMethodId <= 0)
                return BadRequest("PaymentMethodId is required. Call /api/payment/methods first.");

            var nameParts = request.CustomerName.Split(' ', 2);
            var firstName = nameParts[0];
            var lastName = nameParts.Length > 1 ? nameParts[1] : "Customer";
                var payload = new
                {
                    payment_method_id = request.PaymentMethodId,
                    cartTotal = request.Amount.ToString("0.00"),
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
                        successUrl = "https://defame-detonator-aluminum.ngrok-free.dev/api/payment/success",
                        failUrl = "https://defame-detonator-aluminum.ngrok-free.dev/api/payment/fail",
                        pendingUrl = "https://defame-detonator-aluminum.ngrok-free.dev/api/payment/pending"
                    },
                    cartItems = new[]
                    {
                new
                {
                    name = request.ProductName,
                    price = request.Amount.ToString("0.00"),
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
    }
}
