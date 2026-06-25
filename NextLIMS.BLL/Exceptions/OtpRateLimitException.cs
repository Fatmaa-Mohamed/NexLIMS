namespace NextLIMS.BLL.Exceptions
{
    public sealed class OtpRateLimitException : Exception
    {
        public int RetryAfterSeconds { get; }

        public OtpRateLimitException(
            string message,
            int retryAfterSeconds)
            : base(message)
        {
            RetryAfterSeconds =
                Math.Max(1, retryAfterSeconds);
        }
    }
}