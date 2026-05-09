namespace BioDomes.Web.Services;

public interface IImagePathResolver
{
    string UseImageWithDefault(string? fileName, string defaultImageName);
    Task<string> SaveImage(IFormFile file, IWebHostEnvironment env);
    void DeleteImage(string imageName, IWebHostEnvironment env);
}