namespace NextLIMS.BLL.DTO.Client
{
    public class ClientSampleDto
    {
        public int SampleId { get; set; }

        public string SampleName { get; set; } = string.Empty;

        public string SampleType { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime RegisteredAt { get; set; }

        public bool CanDownloadReport { get; set; }
    }
}