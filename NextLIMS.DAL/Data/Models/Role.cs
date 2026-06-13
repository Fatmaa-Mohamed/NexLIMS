namespace NextLIMS.DAL.Data.Models
{
    public class Role
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }

        public Tenant? Tenant { get; set; }
        public ICollection<User> ?Users { get; set; }
        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
