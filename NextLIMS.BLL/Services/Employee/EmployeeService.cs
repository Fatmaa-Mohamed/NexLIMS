using Azure.Core;
using Microsoft.AspNetCore.Http;
using NexLIMS.BLL.DTO.Invite;
using NextLIMS.BLL.DTO.ForgetPassword;
using NextLIMS.BLL.Services.Invitation;
using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.Repositories;

namespace NextLIMS.BLL.Services.EmployeeService
{
    public class EmployeeService
    {
        private readonly InvitationService _invitationService;
        private readonly EmployeeRepository _employeeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmployeeService(
            InvitationService invitationService,
            EmployeeRepository employeeRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _invitationService = invitationService;
            _employeeRepository = employeeRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int?> InviteEmployeeAsync(InviteDTo request)
        {
            return await _invitationService
                .InviteEmployeeAsync(request.Email, request.RoleId);
        }
        public async Task<(bool Success, string Message)> resetPasswordAsync(ResetPassword request)
        {
            var invitation = await _employeeRepository
             .GetPasswordResetWithUserAsync(request.token);

            if (invitation == null || invitation.IsUsed)
                return (false, "Invalid or already used token.");

            if (invitation.ExpiresAt < DateTime.UtcNow)
                return (false, "Token has expired.");

            invitation.user.PasswordHash =
              BCrypt.Net.BCrypt.HashPassword(request.Password);
            invitation.user.IsActive = true;
            invitation.IsUsed = true;

            await _employeeRepository.SaveChangesAsync();

            return (true, "Password reseted successfully.");
        }
        public async Task<(bool Success, string Message)> SetPasswordAsync(SetPassword request)
        {
            var invitation = await _employeeRepository
                .GetPasswordResetWithUserAsync(request.Token);

            if (invitation == null || invitation.IsUsed)
                return (false, "Invalid or already used token.");

            if (invitation.ExpiresAt < DateTime.UtcNow)
                return (false, "Token has expired.");

            invitation.user.PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            invitation.user.Name = request.username;
            invitation.user.IsActive = true;
            invitation.IsUsed = true;
            
            await _employeeRepository.SaveChangesAsync();

            return (true, "Password set successfully.");
        }

        public async Task<object> GetEmployeesByTenantAsync(int tenantId)
        {
            var employees = await _employeeRepository
                .GetEmployeesByTenantAsync(tenantId);

            return employees.Select(u => new
            {
                u.Id,
                u.Email,
                u.Name,
                u.IsActive,
                u.RoleId,
                RoleName = u.Role.Name,
                Permissions = u.Role.RolePermissions
                    .Select(rp => rp.Permission.Name)
                    .ToList()
            });
        }

        public async Task<bool> ForgetPasswordAsync(string email)
        {
            var user = await _employeeRepository
                .GetUserByEmailAsync(email);

            if (user == null)
                return false;

            return await _invitationService
                .EmployeeForgetPasswordAsync(email);
        }

        public int tenantId=> int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("TenantId").Value);
        public async Task<bool> deleteEmployee(int id)
        {
            var result = await _employeeRepository.deleteEmployeeAsync(id, tenantId);
            if (result)
            {
                return true;
            }
            return false;
        }
        
    }
}