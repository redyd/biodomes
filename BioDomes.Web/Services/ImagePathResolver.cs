namespace BioDomes.Web.Services;

public class ImagePathResolver(IHttpContextAccessor httpContextAccessor) : IImagePathResolver
{
    private static readonly string BasePathForUploads = Path.Combine("img", "uploads");
    private static readonly string BasePathForDefault = Path.Combine("img", "defaults");

    private string PathBase => 
        httpContextAccessor.HttpContext?.Request.PathBase ?? string.Empty;

    public string UseImageWithDefault(string? fileName, string defaultImageName)
    {
        return string.IsNullOrWhiteSpace(fileName)
            ? UseStaticImage(defaultImageName)
            : UseUploadedFromId(fileName);
    }
    
    public async Task<string> SaveImage(IFormFile file, IWebHostEnvironment env)
    {
        var uuid = Guid.NewGuid().ToString();
        var filename = $"{uuid}{Path.GetExtension(file.FileName)}";

        var directoryPath = Path.Combine(env.WebRootPath, "img", "uploads");

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var targetLocation = Path.Combine(directoryPath, filename);

        await using var fileStream = File.Create(targetLocation);
        await file.CopyToAsync(fileStream);

        return filename;
    }
    
    public void DeleteImage(string imageName, IWebHostEnvironment env)
    {
        var targetLocation = GetFullPathOfFile(imageName, env);

        if (!File.Exists(targetLocation)) return;

        File.Delete(targetLocation);
    }
    
    private string GetFullPathOfFile(string fileName, IWebHostEnvironment env)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("Invalid file name", nameof(fileName));
        }

        var safeFileName = Path.GetFileName(fileName);

        return Path.Combine(env.WebRootPath, BasePathForUploads, safeFileName);
    }

    private string UseUploadedFromId(string fileName)
    {
        var safeFileName = Path.GetFileName(fileName);
        return $"{PathBase}/{BasePathForUploads}/{safeFileName}".Replace("\\", "/");
    }

    private string UseStaticImage(string fileName)
    {
        var safeFileName = Path.GetFileName(fileName);
        return $"{PathBase}/{BasePathForDefault}/{safeFileName}".Replace("\\", "/");
    }
}