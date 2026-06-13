using System.ComponentModel.DataAnnotations;

namespace NextLIMS.BLL.DTO.TestDto
{
    public class SelectTenantTestRequestDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "testId must be a positive integer.")]
        public int TestId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "standardMethod is required.")]
        [MinLength(1, ErrorMessage = "standardMethod cannot be empty.")]
        public string StandardMethod { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "turnaroundTime must be a positive integer.")]
        public int TurnaroundTime { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "sampleTypeIds must contain at least one id.")]
        public List<int> SampleTypeIds { get; set; } = new();
    }

    public class SelectTenantTestResponseDto
    {
        public int TenantTestId { get; set; }
        public int TestId { get; set; }
        public string TestName { get; set; }
        public string TestType { get; set; }
        public decimal Price { get; set; }
        public string StandardMethod { get; set; }
        public int? TurnaroundTime { get; set; }
        public List<SampleTypeDto> SampleTypes { get; set; } = new();
    }
}