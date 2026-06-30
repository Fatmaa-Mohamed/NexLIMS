using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NextLIMS.BLL.Services.EmailService;
using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.Repositories;
using System.Security.Claims;
using System.Security.Cryptography;

namespace NextLIMS.BLL.Services.Invitation
{
    public class InvitationService
    {
        private readonly InvitationRepository _repository;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InvitationService(
            InvitationRepository repository,
            IEmailService emailService,
            IConfiguration config,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _emailService = emailService;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int?> InviteEmployeeAsync(string email, int roleId)
        {
            var tenantClaim = _httpContextAccessor.HttpContext?
                .User.FindFirst("TenantId")?.Value;

            var userIdClaim = _httpContextAccessor.HttpContext?
                .User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(tenantClaim, out var tenantId)) return null;
            if (!int.TryParse(userIdClaim, out var createdBy)) return null;

            using var transaction = await _repository.BeginTransactionAsync();
            try
            {
                var roleWithPermission = await _repository.getRoleWithItsPermissions(roleId);

                var newrole = new Role
                {
                    TenantId = tenantId,
                    Name = roleWithPermission.Name,
                    Description = roleWithPermission.Description,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy,
                    IsActive = true
                };

                await _repository.addRoleAsync(newrole);
                await _repository.SaveChangesAsync();

                var rolePermissions = roleWithPermission.RolePermissions
                    .Select(rp => new RolePermission
                    {
                        RoleId = newrole.Id,
                        PermissionId = rp.PermissionId,
                        GrantedAt = DateTime.UtcNow,
                        GrantedBy = createdBy
                    })
                    .ToList();

                await _repository.AddRolePermissionsAsync(rolePermissions);

                var user = new User
                {
                    Email = email,
                    Name = string.Empty,
                    RoleId = newrole.Id,
                    TenantId = tenantId,
                    PasswordHash = string.Empty,
                    IsActive = false,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                };

                await _repository.AddUserAsync(user);
                await _repository.SaveChangesAsync();

                var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));

                var passwordReset = new DAL.Data.Models.PasswordReset
                {
                    UserId = user.Id,
                    Token = token,
                    ExpiresAt = DateTime.UtcNow.AddHours(48),
                    IsUsed = false
                };

                await _repository.AddPasswordResetAsync(passwordReset);
                await _repository.SaveChangesAsync();

                var appUrl = _config["App:FrontendUrl"];
                var link = $"{appUrl}/activate?token={token}";
                var body = $@"
                    <h3>Welcome to the team!</h3>
                    <p>Your account has been created with the role:
                    <strong>{newrole.Name}</strong></p>
                    <p>Please click the link below to set your password.
                    This link expires in 48 hours.</p>
                    <a href='{link}'>Set My Password</a>
                ";

                await _emailService.SendAsync(email, "Set Your Password", body);

                await transaction.CommitAsync();
                return user.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.Error.WriteLine($"[InviteEmployee] FAILED: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        public async Task<bool> EmployeeForgetPasswordAsync(string email)
        {
            try
            {
                var user = await _repository.GetUserByEmailAsync(email);

                if (user == null)
                    return false;

                var token = Convert.ToHexString(
                    RandomNumberGenerator.GetBytes(32));

                var passwordReset = new DAL.Data.Models.PasswordReset
                {
                    UserId = user.Id,
                    Token = token,
                    ExpiresAt = DateTime.UtcNow.AddHours(48),
                    IsUsed = false
                };

                await _repository.AddPasswordResetAsync(passwordReset);
                await _repository.SaveChangesAsync();

                var appUrl = _config["App:FrontendUrl"];
                var link = $"{appUrl}/reset-password?token={token}";

                var body = $@"
                    <h3>Password Reset</h3>

                    <p>Please click the link below to reset your password.
                    This link expires in 48 hours.</p>

                    <a href='{link}'>Reset Password</a>
                ";

                await _emailService.SendAsync(
                    email,
                    "Reset Your Password",
                    body);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}