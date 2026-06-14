namespace NextLIMS.BLL.DTO.SampleDTO
{
    public class SampleWithTestsDTO
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int ClientId { get; set; }

        public string SampleName { get; set; }
        public string SampleType { get; set; }
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }

        public List<SampleTestDTO> Tests { get; set; } = new();
    }

    public class SampleTestDTO
    {
        public int Id { get; set; }

        public int ?TenantTestId { get; set; }

        public string TestName { get; set; }

        public string Status { get; set; }

        public string? Result { get; set; }

        public int? AssignedToUserId { get; set; }

        public string? AssignedToUserName { get; set; }

        public int? ApprovedBy { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}