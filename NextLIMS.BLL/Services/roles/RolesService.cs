using Microsoft.AspNetCore.Http;
using NexLIMS.BLL.DTO.RoleDto;
using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.Repositories;
using System.Security.Claims;

namespace NextLIMS.BLL.Services.Roles
{
    public class RoleService
    {
        private readonly RoleRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RoleService(
            RoleRepository repository,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        private int TenantId =>
            int.Parse(_httpContextAccessor.HttpContext!
                .User.FindFirst("TenantId")!.Value);

        private int UserId =>
            int.Parse(_httpContextAccessor.HttpContext!
                .User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        public async Task<Role> CreateRole(CreateRoleDTO dto)
        {
            var role = new Role
            {
                Name = dto.Name,
                Description = dto.Description,
                TenantId = TenantId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = UserId
            };

            await _repository.AddRoleAsync(role);
            await _repository.SaveChangesAsync();

            return role;
        }

        public async Task<List<Role>> GetRoles()
        {
            return await _repository
                .GetRolesByTenantAsync(TenantId);
        }

        public async Task<string> AttachPermissions(
            int roleId,
            ICollection<int> permissionIds)
        {
            var role = await _repository
                .GetRoleByIdAndTenantAsync(
                    roleId,
                    TenantId);

            if (role == null)
                return "RoleNotFound";

            var existingPermissions =
                await _repository.GetExistingPermissionIdsAsync(
                    permissionIds);

            if (existingPermissions.Count != permissionIds.Count)
                return "PermissionNotFound";

            var attachedPermissions =
                await _repository.GetAttachedPermissionIdsAsync(
                    roleId);

            var permissionsToAttach =
                existingPermissions
                    .Except(attachedPermissions)
                    .ToList();

            if (!permissionsToAttach.Any())
                return "AlreadyAttached";

            var rolePermissions =
                permissionsToAttach.Select(id =>
                    new RolePermission
                    {
                        RoleId = roleId,
                        PermissionId = id
                    });

            await _repository
                .AddRolePermissionsAsync(rolePermissions);

            await _repository.SaveChangesAsync();

            return "Success";
        }

        public async Task<bool> DetachPermission(
            int roleId,
            int permissionId)
        {
            var rolePermission =
                await _repository.GetRolePermissionAsync(
                    roleId,
                    permissionId);

            if (rolePermission == null)
                return false;

            _repository.RemoveRolePermission(rolePermission);

            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<object> GetRolePermissions(
            int roleId)
        {
            return await _repository
                .GetRolePermissionsAsync(roleId);
        }
    }
}