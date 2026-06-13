using NexLIMS.BLL.DTO.PermissionDto;
using NextLIMS.DAL.Data.Models;

namespace NexLIMS.BLL.DTO.RoleDto
{
    public static class RoleMappingExtensions
    {
        public static RoleDTO ToDTO(this Role role)
        {
            if (role == null) return null;

            return new RoleDTO
            {
                Id = role.Id,
                TenantId = role.TenantId,
                Name = role.Name,
                Description = role.Description,
                IsActive = role.IsActive,
                CreatedAt = role.CreatedAt,
                Permissions = role.RolePermissions?
                       .Where(rp => rp.Permission != null)
                       .Select(rp => new PermissionDTO
                       {
                           Id = rp.Permission.Id,
                           Name = rp.Permission.Name,
                           Description = rp.Permission.Description
                       })
                       .ToList()
            };
        }

        public static IEnumerable<RoleDTO> ToDTOList(this IEnumerable<Role> roles)
        {
            return roles?.Select(r => r.ToDTO());
        }
    }

 
}