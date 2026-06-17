namespace NextLIMS.BLL.Services.ClientPortal
{
    public interface IWhatsAppService
    {
        Task<string> SendMessageAsync(
            string phoneNumber,
            string message,
            CancellationToken cancellationToken = default);
    }
}