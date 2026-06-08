namespace NexLIMS.API.Data.Models
{
    public class Client
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string Name { get; set; }
        public string NID { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }

        public Tenant Tenant { get; set; }
        public ICollection<Sample> Samples { get; set; }
    }
}
