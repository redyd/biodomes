using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.PageModels;

public abstract class AppPageModel(IWebHostEnvironment environment) : PageModel
{
    public void DeleteImage(string imageName)
    {
        var targetLocation = Path.Combine(environment.ContentRootPath, "wwwroot", "img", "uploads", imageName);
        if (!System.IO.File.Exists(targetLocation)) return;
        System.IO.File.Delete(targetLocation);
    }

    public async Task<string> SaveImage(IFormFile file)
    {
        var uuid = Guid.NewGuid().ToString();
        var filename = $"{uuid}{Path.GetExtension(file.FileName)}";
        var targetLocation = Path.Combine(environment.ContentRootPath, "wwwroot", "img", "uploads", filename);
        await using var fileStream = System.IO.File.Create(targetLocation);
        await file.CopyToAsync(fileStream);

        return filename;
    }

    // public void SuccessNotification(string message)
    // {
    //     TempData.PushNotification(new Notification
    //     {
    //         Type = NotificationType.Success,
    //         Message = message,
    //     });
    // }
    //
    // public void WarningNotification(string message)
    // {
    //     TempData.PushNotification(new Notification
    //     {
    //         Type = NotificationType.Warning,
    //         Message = message,
    //     });
    // }
    //
    // public void ErrorNotification(string message)
    // {
    //     TempData.PushNotification(new Notification
    //     {
    //         Type = NotificationType.Error,
    //         Message = message,
    //     });
    // }
}