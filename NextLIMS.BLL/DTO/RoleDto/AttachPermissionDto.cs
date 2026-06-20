namespace NexLIMS.BLL.DTO.RoleDto
{
    public class AttachPermissionDto
    {
        public ICollection<int> PermissionIds { get; set; } =new HashSet<int>();

    }
}
