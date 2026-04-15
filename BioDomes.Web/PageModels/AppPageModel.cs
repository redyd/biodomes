using BioDomes.Web.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.PageModels;

public abstract class AppPageModel(IWebHostEnvironment environment) : PageModel
{
    public static string BasePathForUploads = Path.Combine("img", "uploads");
    public static string BasePathForDefault = Path.Combine("img", "defaults");

    protected string UseUploadedFromId(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException(null, nameof(fileName));
        }

        var safeFileName = Path.GetFileName(fileName);

        return $"{Request.PathBase}/{BasePathForUploads}/{safeFileName}".Replace("\\", "/");
    }

    protected string GetFullPathOfFile(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("Invalid file name", nameof(fileName));
        }

        var safeFileName = Path.GetFileName(fileName);

        return Path.Combine(environment.WebRootPath, BasePathForUploads, safeFileName);
    }

    protected string GetDefaultDirectoryPath()
    {
        return Path.Combine(environment.WebRootPath, BasePathForDefault);
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

        var directoryPath = Path.Combine(environment.WebRootPath, "img", "uploads");

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

    protected string FormatGram(long grams)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(grams);

        string[] units = { "gramme", "kilogramme", "tonne", "kilotonne", "mégatonne" };

        double value = grams;
        var unitIndex = 0;

        while (value >= 1000 && unitIndex < units.Length - 1)
        {
            value /= 1000;
            unitIndex++;
        }

        var unit = units[unitIndex];

        if (value > 1)
        {
            unit += "s";
        }

        return $"{value:0.##} {unit}";
    }

    protected string FormatCentimeter(long centimeters)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(centimeters);
        var tolerance = 0.1;
        double value;
        string unit;

        if (centimeters >= 100)
        {
            value = centimeters / 100.0;
            unit = Math.Abs(value - 1) < tolerance ? "mètre" : "mètres";
        }
        else
        {
            value = centimeters;
            unit = Math.Abs(value - 1) < tolerance ? "centimètre" : "centimètres";
        }

        return $"{value:0.##} {unit}";
    }
}