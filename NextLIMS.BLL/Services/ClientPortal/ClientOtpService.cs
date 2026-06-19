using Microsoft.Extensions.Options;
using NextLIMS.BLL.DTO.ClientPortal;
using NextLIMS.BLL.Exceptions;
using NextLIMS.BLL.Services.Auth;
using NextLIMS.BLL.Settings;
using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.Repository.ClientPortal;
using NextLIMS.DAL.Repository.ClientRepo;
using NextLIMS.DAL.Repository.TenantRepo;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NextLIMS.BLL.Services.ClientPortal
{
    public class ClientOtpService : IClientOtpService
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IClientPortalRepository _clientPortalRepository;
        private readonly IWhatsAppService _whatsAppService;
        private readonly IJwtAuthenticationService _jwtAuthenticationService;
        private readonly ClientOtpSettings _otpSettings;
        public ClientOtpService(
            ITenantRepository tenantRepository,
            IClientRepository clientRepository,
            IClientPortalRepository clientPortalRepository,
            IWhatsAppService whatsAppService,
            IJwtAuthenticationService jwtAuthenticationService,
            IOptions<ClientOtpSettings> otpSettings)
        {
            _tenantRepository = tenantRepository;
            _clientRepository = clientRepository;
            _clientPortalRepository = clientPortalRepository;
            _whatsAppService = whatsAppService;
            _jwtAuthenticationService = jwtAuthenticationService;
            _otpSettings = otpSettings.Value;

            ValidateSettings();
        }

        public async Task<RequestClientOtpResponseDto> RequestOtpAsync(
            string tenantSlug,
            RequestClientOtpDto request,
            CancellationToken cancellationToken = default)
        {
            var (tenant, client) =
                await GetTenantAndClientAsync(
                    tenantSlug,
                    request.NationalId,
                    "Invalid tenant or client details.",
                    cancellationToken);

            await EnsureOtpIssueAllowedAsync(
                tenant.Id,
                client.Id,
                cancellationToken);

            return await IssueOtpAsync(
                tenant,
                client,
                cancellationToken);
        }

        public async Task<RequestClientOtpResponseDto> ResendOtpAsync(
            string tenantSlug,
            ResendClientOtpDto request,
            CancellationToken cancellationToken = default)
        {
            var (tenant, client) =
                await GetTenantAndClientAsync(
                    tenantSlug,
                    request.NationalId,
                    "Invalid resend details.",
                    cancellationToken);

            var latestOtp =
                await _clientPortalRepository.GetLatestOtpAsync(
                    tenant.Id,
                    client.Id,
                    cancellationToken);

            if (latestOtp == null ||
                latestOtp.Id != request.VerificationId ||
                latestOtp.VerifiedAt.HasValue)
            {
                throw new UnauthorizedAccessException(
                    "Invalid resend details.");
            }

            await EnsureOtpIssueAllowedAsync(
                tenant.Id,
                client.Id,
                cancellationToken,
                latestOtp);

            return await IssueOtpAsync(
                tenant,
                client,
                cancellationToken);
        }

        public async Task<VerifyClientOtpResponseDto> VerifyOtpAsync(
            string tenantSlug,
            VerifyClientOtpDto request,
            CancellationToken cancellationToken = default)
        {

            var (tenant, client) =
                await GetTenantAndClientAsync(
                    tenantSlug,
                    request.NationalId,
                    "Invalid verification details.",
                    cancellationToken);

            var verification =
                await _clientPortalRepository.GetOtpAsync(
                    request.VerificationId,
                    tenant.Id,
                    client.Id,
                    cancellationToken);

            if (verification == null)
            {
                throw new UnauthorizedAccessException(
                    "Invalid verification details.");
            }

            if (verification.IsUsed)
            {
                throw new UnauthorizedAccessException(
                    "The verification code is no longer valid.");
            }

            if (verification.ExpiresAt <= DateTime.UtcNow)
            {
                verification.IsUsed = true;

                await _clientPortalRepository.SaveChangesAsync(
                    cancellationToken);

                throw new UnauthorizedAccessException(
                    "The verification code has expired.");
            }

            if (verification.AttemptCount >=
                verification.MaxAttempts)
            {
                verification.IsUsed = true;

                await _clientPortalRepository.SaveChangesAsync(
                    cancellationToken);

                throw new UnauthorizedAccessException(
                    "Maximum verification attempts reached.");
            }

            verification.AttemptCount++;

            var submittedHash = HashOtp(
                tenant.Id,
                client.Id,
                request.Code);

            var isCorrect = CompareHashes(
                verification.CodeHash,
                submittedHash);

            if (!isCorrect)
            {
                if (verification.AttemptCount >=
                    verification.MaxAttempts)
                {
                    verification.IsUsed = true;
                }

                await _clientPortalRepository.SaveChangesAsync(
                    cancellationToken);

                throw new UnauthorizedAccessException(
                    "Invalid verification code.");
            }

            verification.IsUsed = true;
            verification.VerifiedAt = DateTime.UtcNow;

            await _clientPortalRepository.SaveChangesAsync(
                cancellationToken);

            var token =
                _jwtAuthenticationService.GenerateToken(
                    CreateClientClaims(
                        tenant,
                        client));

            return new VerifyClientOtpResponseDto
            {
                Token = token
            };
        }

        private async Task<RequestClientOtpResponseDto> IssueOtpAsync(
            Tenant tenant,
            Client client,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(
                    client.PhoneNumber))
            {
                throw new InvalidOperationException(
                    "The client does not have a phone number.");
            }
            await _clientPortalRepository
                .InvalidateUnusedOtpsAsync(
                    tenant.Id,
                    client.Id,
                    cancellationToken);

            var now = DateTime.UtcNow;
            var otpCode = GenerateOtpCode();

            var verification =
                new ClientOtpVerification
                {
                    TenantId = tenant.Id,
                    ClientId = client.Id,

                    CodeHash = HashOtp(
                        tenant.Id,
                        client.Id,
                        otpCode),

                    CreatedAt = now,

                    ExpiresAt = now.AddMinutes(
                        _otpSettings.ExpiryMinutes),

                    AttemptCount = 0,
                    MaxAttempts = _otpSettings.MaxAttempts,
                    IsUsed = false
                };

            await _clientPortalRepository.AddOtpAsync(
                verification,
                cancellationToken);

            await _clientPortalRepository.SaveChangesAsync(
                cancellationToken);

            var message = BuildOtpMessage(
                tenant.Name,
                otpCode,
                _otpSettings.ExpiryMinutes);

            try
            {
                verification.TwilioMessageSid =
                    await _whatsAppService.SendMessageAsync(
                        client.PhoneNumber,
                        message,
                        cancellationToken);

                await _clientPortalRepository.SaveChangesAsync(
                    cancellationToken);
            }
            catch
            {

                verification.IsUsed = true;

                await _clientPortalRepository.SaveChangesAsync(
                    cancellationToken);

                throw;
            }

            return new RequestClientOtpResponseDto
            {
                VerificationId = verification.Id,
                ExpiresAt = verification.ExpiresAt,

                MaskedPhoneNumber =
                    MaskPhoneNumber(client.PhoneNumber),

                Message =
                    "A verification code was sent through WhatsApp."
            };
        }

        private async Task EnsureOtpIssueAllowedAsync(
            int tenantId,
            int clientId,
            CancellationToken cancellationToken,
            ClientOtpVerification? latestOtp = null)
        {
            var now = DateTime.UtcNow;

            latestOtp ??=
                await _clientPortalRepository
                    .GetLatestOtpAsync(
                        tenantId,
                        clientId,
                        cancellationToken);

            if (latestOtp != null)
            {
                var nextAllowedRequest =
                    latestOtp.CreatedAt.AddSeconds(
                        _otpSettings.ResendCooldownSeconds);

                if (nextAllowedRequest > now)
                {
                    var retryAfterSeconds =
                        (int)Math.Ceiling(
                            (nextAllowedRequest - now)
                            .TotalSeconds);

                    throw new OtpRateLimitException(
                        $"Please wait {retryAfterSeconds} " +
                        "seconds before requesting another OTP.",
                        retryAfterSeconds);
                }
            }

            var createdSince = now.AddHours(-1);

            var requestsInLastHour =
                await _clientPortalRepository
                    .CountOtpsCreatedSinceAsync(
                        tenantId,
                        clientId,
                        createdSince,
                        cancellationToken);

            if (requestsInLastHour >=
                _otpSettings.MaxRequestsPerHour)
            {
                throw new OtpRateLimitException(
                    "The maximum number of OTP requests " +
                    "has been reached. Please try again later.",
                    3600);
            }
        }

        private async Task<(Tenant Tenant, Client Client)>
            GetTenantAndClientAsync(
                string tenantSlug,
                string nationalId,
                string errorMessage,
                CancellationToken cancellationToken)
        {
            var tenant =
                await _tenantRepository.GetActiveBySlugAsync(
                    tenantSlug,
                    cancellationToken);

            if (tenant == null)
            {
                throw new UnauthorizedAccessException(
                    errorMessage);
            }

            var client =
                await _clientRepository
                    .GetByNationalIdAndTenantAsync(
                        tenant.Id,
                        nationalId,
                        cancellationToken);

            if (client == null)
            {
                throw new UnauthorizedAccessException(
                    errorMessage);
            }

            return (tenant, client);
        }

        private static IEnumerable<Claim> CreateClientClaims(
            Tenant tenant,
            Client client)
        {
            return new List<Claim>
            {
                new(
                    ClaimTypes.Name,
                    client.Name),

                new(
                    "ClientId",
                    client.Id.ToString()),

                new(
                    "TenantId",
                    tenant.Id.ToString()),

                new(
                    "TenantSlug",
                    tenant.Slug),

                new(
                    "ActorType",
                    "Client")
            };
        }

        private void ValidateSettings()
        {
            if (_otpSettings.ExpiryMinutes <= 0)
            {
                throw new InvalidOperationException(
                    "OTP ExpiryMinutes must be greater than zero.");
            }

            if (_otpSettings.MaxAttempts <= 0)
            {
                throw new InvalidOperationException(
                    "OTP MaxAttempts must be greater than zero.");
            }

            if (_otpSettings.ResendCooldownSeconds <= 0)
            {
                throw new InvalidOperationException(
                    "OTP ResendCooldownSeconds must be greater than zero.");
            }

            if (_otpSettings.MaxRequestsPerHour <= 0)
            {
                throw new InvalidOperationException(
                    "OTP MaxRequestsPerHour must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(
                    _otpSettings.Pepper))
            {
                throw new InvalidOperationException(
                    "OTP Pepper is not configured.");
            }
        }

        private static string GenerateOtpCode()
        {
            var number =
                RandomNumberGenerator.GetInt32(
                    0,
                    1_000_000);

            return number.ToString("D6");
        }

        private string HashOtp(
            int tenantId,
            int clientId,
            string otpCode)
        {
            var key = Encoding.UTF8.GetBytes(
                _otpSettings.Pepper);

            var value = Encoding.UTF8.GetBytes(
                $"{tenantId}:{clientId}:{otpCode}");

            using var hmac =
                new HMACSHA256(key);

            return Convert.ToHexString(
                hmac.ComputeHash(value));
        }

        private static bool CompareHashes(
            string storedHash,
            string submittedHash)
        {
            try
            {
                var storedBytes =
                    Convert.FromHexString(storedHash);

                var submittedBytes =
                    Convert.FromHexString(
                        submittedHash);

                return CryptographicOperations
                    .FixedTimeEquals(
                        storedBytes,
                        submittedBytes);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static string BuildOtpMessage(
            string tenantName,
            string otpCode,
            int expiryMinutes)
        {
            return $"""
                {tenantName}

                Your NexLIMS verification code is: {otpCode}

                This code expires in {expiryMinutes} minutes.
                Do not share this code with anyone.
                """;
        }

        private static string MaskPhoneNumber(
            string phoneNumber)
        {
            var digits = new string(
                phoneNumber
                    .Where(char.IsDigit)
                    .ToArray());

            if (digits.Length <= 4)
                return "****";

            return $"******{digits[^4..]}";
        }
    }
}