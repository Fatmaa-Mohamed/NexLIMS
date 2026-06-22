using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexLIMS.API.Middlewares;
using NextLIMS.BLL.DTO;
using NextLIMS.BLL.Services.Tenant;
using NextLIMS.DAL.Data;

namespace NexLIMS.API.Controllers.Tenants_NeedRefactor_
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly TenantSevices _tenantSevices;

        public TenantController(ApplicationDbContext context, TenantSevices tenantSevices)
        {

            _context = context;
            _tenantSevices = tenantSevices;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllTenants()
        {
            var result = await _context.Tenants
                .Select(e => new
                {
                    TenantName = e.Name,
                    TenantId = e.Id
                })
                .ToListAsync();

            return Ok(result);
        }
        [Authorize]
        [HttpGet("profile")]
        [CheckPermission("Profile")]
        public async Task<IActionResult> GetTenantinfo()
        {
            var res = await _tenantSevices.GetTenant();
            return Ok(res);
        }
        [Authorize]
        [HttpPut("profile ")]
        [CheckPermission("Profile")]
        public async Task<IActionResult> UpdateTenant(UpdateTenantDTO updateTenantDTO)
        {
            var res = await _tenantSevices.UpdateTenant(updateTenantDTO);
            if (res > 0)
                return Ok(await _tenantSevices.GetTenant() );
            else
                return BadRequest();
        }
    }
}





