namespace BioDomes.Infrastructures.Models;

public class Specie
{
    // the slug will be the id
    public string Slug { get; set; } = null!;
    
    public string Name { get; set; } = null!;
    public int Height { get; set; }
    public string? UploadedImageId { get; set; }

    public string TypeLabel { get; set; } = null!;
    public string DietLabel { get; set; } = null!;

    // relations
    public SpecieType Type { get; set; } = null!;
    public SpecieDiet Diet { get; set; } = null!;
}
