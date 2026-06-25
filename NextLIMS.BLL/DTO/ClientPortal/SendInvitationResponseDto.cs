namespace NextLIMS.BLL.DTO.ClientPortal
{
    public class SendInvitationResponseDto
    {
        public string Message { get; set; } = string.Empty;

        public string TwilioMessageSid { get; set; } = string.Empty;

        public string PortalLink { get; set; } = string.Empty;
    }
}