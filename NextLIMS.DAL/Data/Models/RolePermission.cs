namespace NextLIMS.DAL.Data.Models
{
    public class RolePermission
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public DateTime GrantedAt { get; set; }
        public int? GrantedBy { get; set; }

        public Role Role { get; set; }
        public Permission Permission { get; set; }
    }
}

