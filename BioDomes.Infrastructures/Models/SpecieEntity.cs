namespace BioDomes.Infrastructures.Models;

public class SpecieEntity
{
    // the slug will be the id
    public string Slug { get; set; } = null!;
    
    public string Name { get; set; } = null!;
    public int Height { get; set; }
    public int Weight { get; set; }
    public string? UploadedImageId { get; set; }

    public string TypeLabel { get; set; } = null!;
    public string DietLabel { get; set; } = null!;
}
