using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace NextLIMS.BLL.Services.ClientPortal
{
    public sealed class DevelopmentWhatsAppService
        : IWhatsAppService
    {
        private readonly ILogger<DevelopmentWhatsAppService>
            _logger;

        public DevelopmentWhatsAppService(
            ILogger<DevelopmentWhatsAppService> logger)
        {
            _logger = logger;
        }

        public Task<string> SendMessageAsync(
            string phoneNumber,
            string message,
            CancellationToken cancellationToken = default)
        {
            cancellationToken
                .ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(
                    phoneNumber))
            {
                throw new ArgumentException(
                    "The phone number is required.",
                    nameof(phoneNumber));
            }

            if (string.IsNullOrWhiteSpace(
                    message))
            {
                throw new ArgumentException(
                    "The WhatsApp message is required.",
                    nameof(message));
            }

            var otpMatch = Regex.Match(
                message,
                @"\b\d{6}\b");

            var isOtpMessage =
                otpMatch.Success;

            var messageType =
                isOtpMessage
                    ? "OTP"
                    : "CLIENT PORTAL INVITATION";

            var otpCode =
                isOtpMessage
                    ? otpMatch.Value
                    : "Not applicable";

            var developmentMessageSid =
                $"DEV-{Guid.NewGuid():N}";

            _logger.LogWarning(
                """
                
                ==========================================================
                NEXLIMS DEVELOPMENT WHATSAPP MESSAGE
                ----------------------------------------------------------
                Message type: {MessageType}
                Phone number: {PhoneNumber}
                OTP code:     {OtpCode}
                Message SID:   {MessageSid}

                Full message:
                {Message}
                ==========================================================
                
                """,
                messageType,
                phoneNumber,
                otpCode,
                developmentMessageSid,
                message);

            return Task.FromResult(
                developmentMessageSid);
        }
    }
}