using Microsoft.AspNetCore.Mvc.Rendering;

namespace BioDomes.Web.Extensions;

public static class NavExtensions
{
    public static string IsActive(this IHtmlHelper html, string pagePrefix)
    {
        var currentPage = html.ViewContext.RouteData.Values["page"]?.ToString();

        return currentPage != null && currentPage.StartsWith(pagePrefix)
            ? "selected-tab"
            : "";
    }
}
