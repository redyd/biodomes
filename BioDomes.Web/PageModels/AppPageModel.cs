using BioDomes.Web.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.PageModels;

public abstract class AppPageModel(IWebHostEnvironment env) : PageModel
{
    public static string BasePathForUploads = Path.Combine("img", "uploads");
    public static string BasePathForDefault = Path.Combine("img", "defaults");

    public string UseImageWithDefault(string? fileName, string defaultImageName)
    {
        return string.IsNullOrWhiteSpace(fileName) 
                ? UseStaticImage(defaultImageName) 
                : UseUploadedFromId(fileName);
    }

    private string UseUploadedFromId(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException(null, nameof(fileName));
        }

        var safeFileName = Path.GetFileName(fileName);

        return $"{Request.PathBase}/{BasePathForUploads}/{safeFileName}".Replace("\\", "/");
    }
    
    private string UseStaticImage(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException(null, nameof(fileName));
        }

        var safeFileName = Path.GetFileName(fileName);

        return $"{Request.PathBase}/{BasePathForDefault}/{safeFileName}".Replace("\\", "/");
    }

    protected string GetFullPathOfFile(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("Invalid file name", nameof(fileName));
        }

        var safeFileName = Path.GetFileName(fileName);

        return Path.Combine(env.WebRootPath, BasePathForUploads, safeFileName);
    }

    protected string GetDefaultDirectoryPath()
    {
        return Path.Combine(env.WebRootPath, BasePathForDefault);
    }

    protected void DeleteImage(string imageName)
    {
        var targetLocation = GetFullPathOfFile(imageName);

        if (!System.IO.File.Exists(targetLocation)) return;

        System.IO.File.Delete(targetLocation);
    }

    protected async Task<string> SaveImage(IFormFile file)
    {
        var uuid = Guid.NewGuid().ToString();
        var filename = $"{uuid}{Path.GetExtension(file.FileName)}";

        var directoryPath = Path.Combine(env.WebRootPath, "img", "uploads");

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var targetLocation = Path.Combine(directoryPath, filename);

        await using var fileStream = System.IO.File.Create(targetLocation);
        await file.CopyToAsync(fileStream);

        return filename;
    }

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