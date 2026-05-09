using BioDomes.Web.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.PageModels;

public abstract class AppPageModel : PageModel
{
    protected void SuccessNotification(string message)
    {
        TempData.PushNotification(new Notification
        {
            Type = NotificationType.Success,
            Message = message,
        });
    }

    protected void ErrorNotification(string message)
    {
        TempData.PushNotification(new Notification
        {
            Type = NotificationType.Error,
            Message = message,
        });
    }
}