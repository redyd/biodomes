namespace BioDomes.Infrastructures.Models;

public class SpecieEntity
{
    public required string Slug { get; set; }
    public required string Name { get; set; }
    public required int Height { get; set; }
    public required int Weight { get; set; }
    public string? UploadedImageId { get; set; }

    public required string TypeLabel { get; set; }
    public required string DietLabel { get; set; }
    
    public required bool IsPublic { get; set; }
}
