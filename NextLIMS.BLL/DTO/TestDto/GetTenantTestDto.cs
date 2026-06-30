namespace NextLIMS.BLL.DTO.TestDto
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class PagedResult<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public List<T> Items { get; set; } = new();
    }

    // Admin response
    public class AdminTenantTestDto
    {
        public int TenantTestId { get; set; }
        public int TestId { get; set; }
        public string TestName { get; set; }
        public string TestType { get; set; }
        public DepartmentDto Department { get; set; }
        public decimal Price { get; set; }
        public string StandardMethod { get; set; }
        public int? TurnaroundTime { get; set; }
        public bool IsActive { get; set; }
        public List<SampleTypeDto> SampleTypes { get; set; } = new();
        public List<ConfirmationTestDto> ConfirmationTests { get; set; } = new();
    }

    // Receptionist response
    public class PublicTenantTestDto
    {
        public int TenantTestId { get; set; }
        public string TestName { get; set; }
        public string TestType { get; set; }
        public DepartmentDto Department { get; set; }
        public decimal Price { get; set; }
        public int? TurnaroundTime { get; set; }
        public List<SampleTypeDto> SampleTypes { get; set; } = new();
    }
}