using System.Text.RegularExpressions;

namespace BioDomes.Web.Extensions;

public static class StringExtensions
{
    public static string Sanitize(this string value)
    {
        return Regex.Replace(value, "<.*?>", String.Empty);
    }
}