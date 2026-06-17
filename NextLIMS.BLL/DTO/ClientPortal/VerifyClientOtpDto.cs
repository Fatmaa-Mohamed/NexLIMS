using System.ComponentModel.DataAnnotations;

namespace NextLIMS.BLL.DTO.ClientPortal
{
    public class VerifyClientOtpDto
    {
        [Required]
        public string NationalId { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "VerificationId must be a positive integer.")]
        public int VerificationId { get; set; }

        [Required]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "OTP must contain exactly 6 digits.")]
        public string Code { get; set; } = string.Empty;
    }
}