using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexLIMS.BLL.DTO.Invite;
using NextLIMS.BLL.DTO.ForgetPassword;
using NextLIMS.BLL.DTO.Invite;
using NextLIMS.BLL.Services.EmployeeService;
using NextLIMS.BLL.Services.Invitation;
using NextLIMS.DAL.Data;
using System.Security.Cryptography;
using System.Text;

namespace NexLIMS.API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("invite")]
        public async Task<IActionResult> InviteEmployee(InviteDTo request)
        {
            var success = await _employeeService.InviteEmployeeAsync(request);

            if (!success)
                return BadRequest("Could not create the user. Email may already exist.");

            return Ok("Invitation sent successfully.");
        }

        [HttpPost("set-password/{token}")]
        public async Task<IActionResult> SetPassword(
        [FromRoute] string token,
        [FromBody] NewPasswordDTO newPasswordAndUsername)
        {
            var request = new SetPassword
            {
                Token = token,
                NewPassword = newPasswordAndUsername.NewPassword,
                username = newPasswordAndUsername.userName
            };

            var result = await _employeeService.SetPasswordAsync(request);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpGet("{tenantId}")]
        public async Task<IActionResult> GetEmployeesByTenant(int tenantId)
        {
            var employees = await _employeeService.GetEmployeesByTenantAsync(tenantId);
            return Ok(employees);
        }

        [HttpPost("forgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDTO dto)
        {
            var result = await _employeeService.ForgetPasswordAsync(dto.Email);
            if (!result)
                return BadRequest("you have no account");
            return Ok("reseted successfully");
        }
    }
}