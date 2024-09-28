namespace my_portfolio_api.Utils
{
    public class StringHelper
    {
        // This method formats strings to have the first letter of each word capitalized
        public static string FormatTitleCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
        }
    }
}
