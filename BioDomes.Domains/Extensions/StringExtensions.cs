using System.Text.RegularExpressions;

namespace BioDomes.Domains.Extensions;

public static class StringExtensions
{
    public static string ToKebabCase(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return value;

        return Regex.Replace(value, @"[\s\p{P}]+", "-").ToLower();
    }
}