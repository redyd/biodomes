namespace BioDomes.Infrastructures.Models;

public class SpecieEntity
{
    public string Slug { get; set; }
    public string Name { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public string? UploadedImageId { get; set; }

    public bool IsPublic { get; set; }

    // user
    public string UserId { get; set; }
    public UserEntity User { get; set; }

    // relation
    public List<BiomeSpecieEntity> BiomeSpecies { get; set; } = new();

    public int DietId { get; set; }
    public SpecieDiet DietEntity { get; set; }

    public int ClassificationId { get; set; }
    public SpecieClassification ClassificationEntity { get; set; }
    
    // food chain
    public List<SpecieFoodChain> Preys { get; set; } = new();
    public List<SpecieFoodChain> Predators { get; set; } = new();
}