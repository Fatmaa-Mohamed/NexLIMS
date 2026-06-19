namespace NextLIMS.BLL.Settings
{
    public class ClientPortalSettings
    {
        public string BaseUrl { get; set; } = string.Empty;

        public string LoginPathTemplate { get; set; } =
            "/api/client-portal/{0}/login";
    }
}