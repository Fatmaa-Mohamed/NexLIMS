using Microsoft.AspNetCore.Mvc;
using NextLIMS.BLL.Services.Permissions;

namespace NexLIMS.API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly PermissionService _permissionService;

        public PermissionController(
            PermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPermissions()
        {
            var permissions = await _permissionService
                .GetAllPermissions();

            return Ok(permissions);
        }
    }
}