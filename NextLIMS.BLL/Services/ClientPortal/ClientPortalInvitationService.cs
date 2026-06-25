using Microsoft.Extensions.Options;
using NextLIMS.BLL.DTO.ClientPortal;
using NextLIMS.BLL.Settings;
using NextLIMS.DAL.Repository.ClientPortal;

namespace NextLIMS.BLL.Services.ClientPortal
{
    public class ClientPortalInvitationService
        : IClientPortalInvitationService
    {
        private readonly IClientPortalRepository
            _clientPortalRepository;

        private readonly IWhatsAppService
            _whatsAppService;

        private readonly ClientPortalSettings
            _portalSettings;

        public ClientPortalInvitationService(
            IClientPortalRepository clientPortalRepository,
            IWhatsAppService whatsAppService,
            IOptions<ClientPortalSettings> portalSettings)
        {
            _clientPortalRepository =
                clientPortalRepository;

            _whatsAppService = whatsAppService;

            _portalSettings = portalSettings.Value;
        }

        public async Task<SendInvitationResponseDto>
            SendInvitationAsync(
                int tenantId,
                SendInvitationRequestDto request,
                CancellationToken cancellationToken = default)
        {
            var sample =
                await _clientPortalRepository
                    .GetInvitationContextAsync(
                        tenantId,
                        request.SampleId,
                        cancellationToken);

            if (sample == null)
            {
                throw new KeyNotFoundException(
                    "The sample was not found for this tenant.");
            }

            if (sample.Client == null)
            {
                throw new InvalidOperationException(
                    "The sample does not have a client.");
            }

            if (sample.Tenant == null)
            {
                throw new InvalidOperationException(
                    "The sample does not have a tenant.");
            }

            if (string.IsNullOrWhiteSpace(
                    sample.Client.PhoneNumber))
            {
                throw new InvalidOperationException(
                    "The client does not have a phone number.");
            }

            var portalLink = BuildPortalLink(
                sample.Tenant.Slug);

            var message = BuildInvitationMessage(
                sample.Client.Name,
                sample.Id,
                sample.Tenant.Name,
                portalLink);

            var messageSid =
                await _whatsAppService.SendMessageAsync(
                    sample.Client.PhoneNumber,
                    message,
                    cancellationToken);

            return new SendInvitationResponseDto
            {
                Message =
                    "Client portal invitation was sent successfully.",

                TwilioMessageSid = messageSid,

                PortalLink = portalLink
            };
        }

        private string BuildPortalLink(string tenantSlug)
        {
            if (string.IsNullOrWhiteSpace(
                    _portalSettings.BaseUrl))
            {
                throw new InvalidOperationException(
                    "Client portal BaseUrl is not configured.");
            }

            var path = string.Format(
                _portalSettings.LoginPathTemplate,
                Uri.EscapeDataString(tenantSlug));

            return
                $"{_portalSettings.BaseUrl.TrimEnd('/')}" +
                $"/{path.TrimStart('/')}";
        }

        private static string BuildInvitationMessage(
            string clientName,
            int sampleId,
            string tenantName,
            string portalLink)
        {
            return $"""
                Hello {clientName},

                Your sample #{sampleId} has been registered at {tenantName}.

                Open your client portal:
                {portalLink}

                Enter your National ID to receive your WhatsApp verification code.
                """;
        }
    }
}