namespace NextLIMS.BLL.DTO.Client
{
    public class ClientMeDto
    {
        public int ClientId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string NationalId { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public int TotalSamples { get; set; }

        public List<ClientSampleStatusSummaryDto>
            SampleStatuses
        { get; set; } = new();
    }

    public class ClientSampleStatusSummaryDto
    {
        public string Status { get; set; } = string.Empty;

        public int Count { get; set; }
    }
}