using NextLIMS.BLL.DTO.ClientPortal;

namespace NextLIMS.BLL.Services.ClientPortal
{
    public interface IClientOtpService
    {
        Task<RequestClientOtpResponseDto> RequestOtpAsync(
            string tenantSlug,
            RequestClientOtpDto request,
            CancellationToken cancellationToken = default);

        Task<RequestClientOtpResponseDto> ResendOtpAsync(
            string tenantSlug,
            ResendClientOtpDto request,
            CancellationToken cancellationToken = default);

        Task<VerifyClientOtpResponseDto> VerifyOtpAsync(
            string tenantSlug,
            VerifyClientOtpDto request,
            CancellationToken cancellationToken = default);
    }
}