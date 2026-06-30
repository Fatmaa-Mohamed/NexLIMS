using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using NextLIMS.BLL.Services.Subscription;

namespace NexLIMS.API.Controllers.Dev
{
    [ApiController]
    [Route("api/dev")]
    public class DevController : ControllerBase
    {
        private readonly SubscriptionPaymentService _paymentService;
        private readonly IHostEnvironment _env;

        public DevController(SubscriptionPaymentService paymentService, IHostEnvironment env)
        {
            _paymentService = paymentService;
            _env = env;
        }

        [HttpPost("simulate-payment")]
        public async Task<IActionResult> SimulatePayment([FromQuery] string invoiceId)
        {
            if (!_env.IsDevelopment())
                return NotFound();

            if (string.IsNullOrWhiteSpace(invoiceId))
                return BadRequest(new { message = "invoiceId is required." });

            var result = await _paymentService.ExecuteAsync(invoiceId);
            return Ok(new { message = result });
        }
    }
}
