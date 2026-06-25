using NextLIMS.BLL.DTO.ClientPortal;

namespace NextLIMS.BLL.Services.ClientPortal
{
    public interface IClientPortalInvitationService
    {
        Task<SendInvitationResponseDto> SendInvitationAsync(
            int tenantId,
            SendInvitationRequestDto request,
            CancellationToken cancellationToken = default);
    }
}