using NexLIMS.BLL.DTO.PermissionDto;

namespace NexLIMS.BLL.DTO.RoleDto
{
    public class RolePermissionDTO
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public PermissionDTO? Permission { get; set; }
    }
}