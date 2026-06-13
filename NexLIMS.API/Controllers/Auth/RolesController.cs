using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexLIMS.API.Middlewares;
using NexLIMS.BLL.DTO.RoleDto;
using NextLIMS.BLL.Services.Roles;


namespace NexLIMS.API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RolesController(RoleService roleService)
        {
            _roleService = roleService;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleDTO dto)
        {
            var role = await _roleService.CreateRole(dto);

            return Ok(role);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await _roleService.GetRoles());
        }

        [Authorize]
        [HttpPost("{roleId}/permissions")]//AttachPermissionsToRole
        public async Task<IActionResult> AttachPermission(
            int roleId,
            AttachPermissionDto dto)
        {
            var result = await _roleService
                .AttachPermissions(roleId, dto.PermissionIds);

            return result switch
            {
                "RoleNotFound" => NotFound("Role not found"),
                "PermissionNotFound" => BadRequest("One or more permissions do not exist"),
                "AlreadyAttached" => BadRequest("All permissions are already attached"),
                _ => Ok()
            };
        }
        [HttpPost("{roleId}/detach-permissions")]
        [Authorize]
        public async Task<IActionResult> DetachPermissions(int roleId, [FromBody] AttachPermissionDto dto)
        {
            var result = await _roleService.DetachPermissions(roleId, dto);

            if (!result)
                return NotFound("Role not found for this tenant, or no matching permissions to detach.");

            return Ok("Permissions detached successfully.");
        }


    }
}
