using System.ComponentModel.DataAnnotations;

namespace NextLIMS.BLL.DTO.TestDto
{
    public class ToggleTenantTestStatusRequestDto
    {
        [Required(ErrorMessage = "isActive is required.")]
        public bool? IsActive { get; set; }
    }

    public class ToggleTenantTestStatusResponseDto
    {
        public int TenantTestId { get; set; }
        public bool IsActive { get; set; }
    }
}