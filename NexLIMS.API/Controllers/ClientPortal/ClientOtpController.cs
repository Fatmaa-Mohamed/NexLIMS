using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextLIMS.BLL.DTO.ClientPortal;
using NextLIMS.BLL.Services.ClientPortal;
using Microsoft.AspNetCore.RateLimiting;
using NextLIMS.BLL.Exceptions;

namespace NexLIMS.API.Controllers.ClientPortal
{
    [ApiController]
    [Route("api/client-portal/{slug}/auth")]
    public class ClientOtpController : ControllerBase
    {
        private readonly IClientOtpService
            _clientOtpService;

        public ClientOtpController(
            IClientOtpService clientOtpService)
        {
            _clientOtpService = clientOtpService;
        }

        [AllowAnonymous]
        [EnableRateLimiting("ClientOtp")]
        [HttpPost("request-otp")]
        public async Task<IActionResult> RequestOtp(
            [FromRoute] string slug,
            [FromBody] RequestClientOtpDto request,
            CancellationToken cancellationToken)
        {
            try
            {
                var result =
                    await _clientOtpService.RequestOtpAsync(
                        slug,
                        request,
                        cancellationToken);

                return Ok(result);
            }
            catch (OtpRateLimitException exception)
            {
                Response.Headers.RetryAfter =
                    exception.RetryAfterSeconds.ToString();

                return StatusCode(
                    StatusCodes.Status429TooManyRequests,
                    new
                    {
                        message = exception.Message,
                        retryAfterSeconds =
                            exception.RetryAfterSeconds
                    });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new
                {
                    message =
                        "Invalid tenant or client details."
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

        [AllowAnonymous]
        [EnableRateLimiting("ClientOtp")]
        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp(
            [FromRoute] string slug,
            [FromBody] ResendClientOtpDto request,
            CancellationToken cancellationToken)
        {
            try
            {
                var result =
                    await _clientOtpService.ResendOtpAsync(
                        slug,
                        request,
                        cancellationToken);

                return Ok(result);
            }
            catch (OtpRateLimitException exception)
            {
                Response.Headers.RetryAfter =
                    exception.RetryAfterSeconds.ToString();

                return StatusCode(
                    StatusCodes.Status429TooManyRequests,
                    new
                    {
                        message = exception.Message,
                        retryAfterSeconds =
                            exception.RetryAfterSeconds
                    });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new
                {
                    message = "Invalid resend details."
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

        [AllowAnonymous]
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(
        [FromRoute] string slug,
        [FromBody] VerifyClientOtpDto request,
        CancellationToken cancellationToken)
        {
            try
            {
                var result =
                    await _clientOtpService.VerifyOtpAsync(
                        slug,
                        request,
                        cancellationToken);

                return Ok(result);
            }
            catch (UnauthorizedAccessException exception)
            {
                return Unauthorized(new
                {
                    message = exception.Message
                });
            }
        }
    }
}