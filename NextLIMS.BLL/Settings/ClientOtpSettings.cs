namespace NextLIMS.BLL.Settings
{
    public class ClientOtpSettings
    {
        public int ExpiryMinutes { get; set; } = 10;

        public int MaxAttempts { get; set; } = 5;

        public string Pepper { get; set; } = string.Empty;
    }
}