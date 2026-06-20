using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexLIMS.API.Middlewares;
using NextLIMS.DAL.Data;

namespace NexLIMS.API.Controllers.Tenants_NeedRefactor_
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public TenantController(ApplicationDbContext context) { 
        
        _context = context;
        }
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
    }
}
