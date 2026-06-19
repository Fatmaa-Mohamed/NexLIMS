using Microsoft.Extensions.Options;
using NextLIMS.BLL.Settings;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace NextLIMS.BLL.Services.ClientPortal
{
    public class TwilioWhatsAppService : IWhatsAppService
    {
        private readonly TwilioSettings _settings;

        public TwilioWhatsAppService(
            IOptions<TwilioSettings> settings)
        {
            _settings = settings.Value;

            ValidateSettings();

            TwilioClient.Init(
                _settings.AccountSid,
                _settings.AuthToken);
        }

        public async Task<string> SendMessageAsync(
            string phoneNumber,
            string message,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException(
                    "WhatsApp message cannot be empty.");
            }

            var normalizedPhoneNumber =
                NormalizePhoneNumber(phoneNumber);

            var result = await MessageResource.CreateAsync(
                from: new PhoneNumber(
                    $"whatsapp:{_settings.WhatsAppSandboxNumber}"),

                to: new PhoneNumber(
                    $"whatsapp:{normalizedPhoneNumber}"),

                body: message);

            return result.Sid;
        }

        private void ValidateSettings()
        {
            if (string.IsNullOrWhiteSpace(
                    _settings.AccountSid))
            {
                throw new InvalidOperationException(
                    "Twilio AccountSid is not configured.");
            }

            if (string.IsNullOrWhiteSpace(
                    _settings.AuthToken))
            {
                throw new InvalidOperationException(
                    "Twilio AuthToken is not configured.");
            }

            if (string.IsNullOrWhiteSpace(
                    _settings.WhatsAppSandboxNumber))
            {
                throw new InvalidOperationException(
                    "Twilio WhatsApp Sandbox number " +
                    "is not configured.");
            }
        }

        private string NormalizePhoneNumber(
            string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new ArgumentException(
                    "Client phone number is missing.");
            }

            var trimmedPhone = phoneNumber.Trim();

            var digits = new string(
                trimmedPhone
                    .Where(char.IsDigit)
                    .ToArray());

            if (trimmedPhone.StartsWith("+"))
                return $"+{digits}";

            if (digits.StartsWith("00"))
                return $"+{digits[2..]}";

            var countryCodeDigits =
                _settings.DefaultCountryCode
                    .Trim()
                    .TrimStart('+');

            if (digits.StartsWith(countryCodeDigits))
                return $"+{digits}";

            if (digits.StartsWith("0"))
                digits = digits[1..];

            return $"+{countryCodeDigits}{digits}";
        }
    }
}