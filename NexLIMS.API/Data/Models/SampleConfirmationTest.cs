namespace NexLIMS.API.Data.Models
{
    public class SampleConfirmationTest
    {
        public int Id { get; set; }
        public int SampleTestId { get; set; }
        public string ConfirmationTestName { get; set; }
        public string Result { get; set; }
        public int PerformedBySeniorAnalystId { get; set; }
        public DateTime DatePerformed { get; set; }

        public SampleTest SampleTest { get; set; }
        public User PerformedBySeniorAnalyst { get; set; }
    }
}
