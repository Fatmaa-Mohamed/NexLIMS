namespace NextLIMS.DAL.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }

        public Tenant Tenant { get; set; }
        public Role Role { get; set; }
    }
}
