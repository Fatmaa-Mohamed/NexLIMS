using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NexLIMS.API.Middlewares;
using NextLIMS.BLL.Services.Subscription;
using System.Numerics;

namespace NexLIMS.API.Controllers.Subscription
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly SubscriptionService _subscriptionService;

        public SubscriptionController(SubscriptionService subscriptionService )
        {
            _subscriptionService = subscriptionService;
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

    }
}
