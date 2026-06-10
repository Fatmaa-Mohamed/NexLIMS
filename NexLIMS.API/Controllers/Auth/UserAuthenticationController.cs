
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NexLIMS.API.DTO;
using NextLIMS.BLL.Services.Auth;
using NextLIMS.DAL.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NexLIMS.API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly UserAuthenticationService userAuthenticationService;
        public UserAuthenticationController(
          UserAuthenticationService userAuthenticationService
            )
        {
            this.userAuthenticationService = userAuthenticationService;
        }


        [HttpPost("login")]
        public async Task< IActionResult> Login(LoginDto dto)
        {
            return Ok(await userAuthenticationService.LoginAsync(dto.Email, dto.Password, dto.tenantId));
        }
    }
}
