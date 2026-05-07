namespace BioDomes.Infrastructures.Models;

public class SpecieEntity
{
    public int Id { get; set; }
    public string Slug { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int Height { get; set; }
    public int Weight { get; set; }
    public string? UploadedImageId { get; set; }

    public bool IsPublic { get; set; }

    // user
    public string UserId { get; set; } = null!;
    public UserEntity User { get; set; } = null!;

    // relation
    public List<BiomeSpecieEntity> BiomeSpecies { get; set; } = new();

    public int DietId { get; set; }
    public SpecieDiet DietEntity { get; set; } = null!;

    public int ClassificationId { get; set; }
    public SpecieClassification ClassificationEntity { get; set; } = null!;
    
    // food chain
    public List<SpecieFoodChain> Preys { get; set; } = new();
    public List<SpecieFoodChain> Predators { get; set; } = new();
}