using System.ComponentModel.DataAnnotations;

namespace NextLIMS.BLL.DTO.DepartmentDto
{
    public class SelectDepartmentRequestDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "departmentId must be a positive integer")]
        public int DepartmentId { get; set; }
    }

    public class SelectDepartmentResponseDto
    {
        public int TenantId { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}