using BioDomes.Domains.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToPageResult<T>(
        this Result<T> result,
        PageModel page)
    {
        return result.Status switch
        {
            ResultStatus.Success => new PageResult(),
            ResultStatus.NotFound => page.RedirectToPage("/Errors/404"),
            ResultStatus.Forbidden => page.RedirectToPage("/Errors/403"),
            ResultStatus.Failure => page.RedirectToPage("/Errors/500"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}