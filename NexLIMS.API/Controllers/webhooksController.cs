using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextLIMS.BLL.DTO;

namespace NexLIMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class webhooksController : ControllerBase
    {
        [HttpPost("fawaterak/paid")]
        public async Task<IActionResult> Fawaterak(PaymentRequestDto paymentResonse)
        {
            var response = paymentResonse;
            Console.WriteLine(response);
            return Ok(paymentResonse);
        }
    }
}
