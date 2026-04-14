using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.PageModels;

public class UserPageModel: PageModel
{
    public int? CurrentUserId()
    {
        int? userId = null;
        if (User.Identity?.IsAuthenticated == true)
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(claim, out var parsed))
            {
                userId = parsed;
            }
        }
        return userId;
    }

    public bool IsAdmin()
    {
        return User.IsInRole("Admin");
    }
}