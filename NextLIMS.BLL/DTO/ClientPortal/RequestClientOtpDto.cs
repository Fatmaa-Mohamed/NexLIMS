using System.ComponentModel.DataAnnotations;

namespace NextLIMS.BLL.DTO.ClientPortal
{
    public class RequestClientOtpDto
    {
        [Required]
        public string NationalId { get; set; } = string.Empty;
    }
}