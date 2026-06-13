using Microsoft.AspNetCore.Http;
using NexLIMS.BLL.DTO.RoleDto;
using NextLIMS.BLL.DTO.RoleDto;
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

        public async Task<CreateRoleResponseDto> CreateRole(CreateRoleDTO dto)
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
            var response = new CreateRoleResponseDto
            {
                Id = role.Id,  // Add this if needed
                Name = role.Name,
                Description = role.Description,
                IsActive = role.IsActive,
                CreatedAt = role.CreatedAt,
                CreatedBy = role.CreatedBy
            };
            return response;
        }

        public async Task<List<RoleDTO>> GetRoles()
        {
            List<Role> result = await _repository.GetRolesByTenantAsync(TenantId);

            return result.ToDTOList().ToList();
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

        public async Task<bool> DetachPermissions(int roleId, AttachPermissionDto dto)
        {
            // Verify the role belongs to the current tenant
            bool roleBelongsToTenant = await _repository.RoleExistsForTenantAsync(roleId, TenantId);

            if (!roleBelongsToTenant)
                return false;

            if (dto.PermissionIds == null || !dto.PermissionIds.Any())
                return false;

            var rolePermissions = await _repository.GetRolePermissionsAsync2(roleId, dto.PermissionIds);

            if (!rolePermissions.Any())
                return false;

            _repository.RemoveRolePermissionsRange(rolePermissions);

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