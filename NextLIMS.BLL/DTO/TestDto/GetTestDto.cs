namespace NextLIMS.BLL.DTO.TestDto
{
    public class GetTestDto
    {
        public int Id { get; set; }
        public string TestName { get; set; }
        public string TestType { get; set; }
        public string StandardMethod { get; set; }
        public int? TurnaroundTime { get; set; }
        public List<SampleTypeDto> SampleTypes { get; set; } = new();
        public List<ConfirmationTestDto> ConfirmationTests { get; set; } = new();
    }

    public class SampleTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ConfirmationTestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}