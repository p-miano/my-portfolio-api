namespace my_portfolio_api.Utils
{
    public static class UrlHelper
    {
        public static bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
}
