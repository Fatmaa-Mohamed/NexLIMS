using System.ComponentModel.DataAnnotations;

namespace NextLIMS.BLL.DTO.ClientPortal
{
    public class ResendClientOtpDto
    {
        [Required]
        public string NationalId { get; set; } = string.Empty;

        [Range(
            1,
            int.MaxValue,
            ErrorMessage =
                "VerificationId must be a positive integer.")]
        public int VerificationId { get; set; }
    }
}