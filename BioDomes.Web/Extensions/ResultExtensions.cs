using BioDomes.Domains.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToPageResult<T>(
        this Result<T> result,
        PageModel page,
        string? notFoundPage = "/Errors/404",
        string? forbiddenPage = "/Errors/403")
    {
        return result.Status switch
        {
            ResultStatus.Success => new PageResult(),
            ResultStatus.NotFound => page.RedirectToPage(notFoundPage),
            ResultStatus.Forbidden => page.RedirectToPage(forbiddenPage),
            ResultStatus.Failure => page.RedirectToPage("/Errors/500"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}