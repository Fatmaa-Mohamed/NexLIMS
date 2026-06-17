namespace NextLIMS.BLL.DTO.ClientPortal
{
    public class RequestClientOtpResponseDto
    {
        public int VerificationId { get; set; }

        public DateTime ExpiresAt { get; set; }

        public string MaskedPhoneNumber { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
    }
}