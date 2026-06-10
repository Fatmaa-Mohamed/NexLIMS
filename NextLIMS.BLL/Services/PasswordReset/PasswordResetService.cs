using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using NextLIMS.BLL.Services.EmailService;
using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.Repositories;
using System.Security.Cryptography;

namespace NextLIMS.BLL.Services.PasswordReset
{
    public class PasswordResetService
    {
        private readonly PasswordResetRepository _repository;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PasswordResetService(
            PasswordResetRepository repository,
            IEmailService emailService,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> SendResetLinkAsync(string email)
        {
            var user = await _repository.GetUserByEmailAsync(email);

            // Prevent email enumeration
            if (user == null || !user.IsActive)
                return true;

            await _repository.RemoveUnusedTokensAsync(user.Id);

            var token = Convert.ToHexString(
                RandomNumberGenerator.GetBytes(32));

            var resetToken = new NextLIMS.DAL.Data.Models.PasswordReset
            {
                UserId = user.Id,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                IsUsed = false
            };

            await _repository.AddPasswordResetAsync(resetToken);
            await _repository.SaveChangesAsync();

            var request = _httpContextAccessor.HttpContext?.Request;
            var baseUrl = $"{request?.Scheme}://{request?.Host}";
            var link = $"{baseUrl}/reset-password?token={token}";

            var body = $@"
                <h3>Password Reset Request</h3>
                <p>We received a request to reset your password.</p>
                <p>Click the link below. This link expires in
                <strong>1 hour</strong>.</p>
                <a href='{link}'>Reset My Password</a>
                <br/><br/>
                <p>If you did not request this, please ignore this email.</p>
            ";

            await _emailService.SendAsync(
                email,
                "Reset Your Password",
                body);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(
            string token,
            string newPassword)
        {
            var resetToken = await _repository
                .GetPasswordResetWithUserAsync(token);

            if (resetToken == null)
                return false;

            if (resetToken.IsUsed)
                return false;

            if (resetToken.ExpiresAt < DateTime.UtcNow)
                return false;

            resetToken.user.PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(newPassword);

            resetToken.IsUsed = true;

            await _repository.SaveChangesAsync();

            return true;
        }
    }
}