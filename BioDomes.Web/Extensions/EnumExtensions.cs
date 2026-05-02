using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BioDomes.Web.Extensions;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum value)
    {
        var member = value.GetType().GetMember(value.ToString()).FirstOrDefault();
        if (member is null) return value.ToString();

        var attr = member.GetCustomAttribute<DisplayAttribute>();
        return attr?.Name ?? value.ToString();
    }
}