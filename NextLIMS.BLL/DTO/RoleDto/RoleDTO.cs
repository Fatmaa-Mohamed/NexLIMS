using NextLIMS.DAL.Data.Models;

namespace NexLIMS.BLL.DTO.RoleDto
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<RolePermission>? RolePermissions { get; set; }

    }
}
