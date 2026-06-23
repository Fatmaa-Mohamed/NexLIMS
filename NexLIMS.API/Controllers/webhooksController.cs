using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NextLIMS.BLL.DTO;
using NextLIMS.DAL.Data;

namespace NexLIMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class webhooksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public webhooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("fawaterak/paid")]
        public async Task<IActionResult> Fawaterak([FromBody] PaymentRequestDto paymentResponse)
        {
            if (paymentResponse?.Status != "paid" || paymentResponse.CustomerData == null)
                return Ok();

            var email = paymentResponse.CustomerData.CustomerEmail;
            if (string.IsNullOrEmpty(email))
                return Ok();

            var user = await _context.Users
                .Include(u => u.Tenant)
                .FirstOrDefaultAsync(u =>
                    u.Email == email &&
                    u.Tenant != null &&
                    u.Tenant.SubscriptionStatus == "PendingPayment");

            if (user?.Tenant == null)
                return Ok();

            user.Tenant.SubscriptionStatus = "Active";
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}