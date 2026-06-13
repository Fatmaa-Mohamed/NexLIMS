namespace NextLIMS.DAL.Data.Models
{
    public class ConfirmationTestTemplate
    {
        public int Id { get; set; }
        public int? TenantId { get; set; }
        public int TestId { get; set; }
        public string ConfirmationTestName { get; set; }

        public Tenant Tenant { get; set; }
        public Test Test { get; set; }
    }
}
