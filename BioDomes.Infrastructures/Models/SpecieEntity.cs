namespace BioDomes.Infrastructures.Models;

public class SpecieEntity
{
    public string Slug { get; set; }
    public string Name { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public string? UploadedImageId { get; set; }

    public string TypeLabel { get; set; }
    public string DietLabel { get; set; }

    public bool IsPublic { get; set; }
    
    // user
    public string UserId { get; set; }
    public UserEntity User { get; set; }

    // relation
    public List<BiomeEntity> Biomes { get; set; } = new();
}