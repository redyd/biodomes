using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace BioDomes.Domains.Extensions;

public static class StringExtensions
{
    public static string ToKebabCase(this string value)
    {
        if (string.IsNullOrEmpty(value)) return value;

        var normalized = value.Normalize(NormalizationForm.FormD);
    
        var withoutDiacritics = new string(
            normalized
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray()
        );
    
        return Regex.Replace(withoutDiacritics, @"[\s\p{P}]", "-").ToLower();
    }
}