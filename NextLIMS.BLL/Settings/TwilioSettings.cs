namespace NextLIMS.BLL.Settings
{
    public class TwilioSettings
    {
        public string AccountSid { get; set; } = string.Empty;

        public string AuthToken { get; set; } = string.Empty;

        public string WhatsAppSandboxNumber { get; set; } =
            string.Empty;

        public string DefaultCountryCode { get; set; } = "+20";
    }
}