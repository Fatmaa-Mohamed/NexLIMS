using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexLIMS.API.Middlewares;
using NextLIMS.BLL.DTO.ClientPortal;
using NextLIMS.BLL.Services.ClientPortal;
using System.Security.Claims;

namespace NexLIMS.API.Controllers.ClientPortal
{
    [ApiController]
    [Route("api/client-portal")]
    [Authorize]
    public class ClientPortalInvitationController
        : ControllerBase
    {
        private readonly IClientPortalInvitationService
            _invitationService;

        public ClientPortalInvitationController(
            IClientPortalInvitationService invitationService)
        {
            _invitationService = invitationService;
        }

        [HttpPost("send-invitation")]
        [CheckPermission("SendClientPortalInvitation")]
        public async Task<IActionResult> SendInvitation(
            [FromBody] SendInvitationRequestDto request,
            CancellationToken cancellationToken)
        {
            var tenantIdClaim =
                User.FindFirstValue("TenantId");

            if (!int.TryParse(
                    tenantIdClaim,
                    out var tenantId))
            {
                return Unauthorized(new
                {
                    message =
                        "The authenticated user does not " +
                        "have a valid TenantId."
                });
            }

            try
            {
                var result =
                    await _invitationService
                        .SendInvitationAsync(
                            tenantId,
                            request,
                            cancellationToken);

                return Ok(result);
            }
            catch (KeyNotFoundException exception)
            {
                return NotFound(new
                {
                    message = exception.Message
                });
            }
            catch (InvalidOperationException exception)
            {
                return BadRequest(new
                {
                    message = exception.Message
                });
            }
        }
    }
}