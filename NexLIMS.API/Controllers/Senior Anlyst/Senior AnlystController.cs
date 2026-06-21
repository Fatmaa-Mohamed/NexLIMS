using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.Repository.DepartmentDirector;

namespace NexLIMS.API.Controllers.Senior_Anlyst
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeniorAnlystController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public SeniorAnlystController(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        public int TenantId => int.Parse(httpContextAccessor.HttpContext.User.FindFirst("TenantId").Value);
        [HttpGet("GetSeniorAnlyst")]
        public async Task<IActionResult> GetSeniorAnlyst()
        {
            var res =  _dbContext.Users.Where(u => u.TenantId == TenantId && u.Name == "Senior Analyst");
            return Ok(res);
        }
        [HttpGet("AssignToSeniorAnalyst")]
        public async Task<IActionResult> AssignToSeniorAnalyst(int sampleid , int analystId)
        {
            var sampleTests = await _dbContext.SampleTests
                            .Where(x => x.SampleId == sampleid &&
                            x.Sample.TenantId == TenantId).ToListAsync();

            foreach (var sampleTest in sampleTests)
            {
                sampleTest.AssignedToUserId = analystId;
            }
            return Ok();
        }

    }
}
