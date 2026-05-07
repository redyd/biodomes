namespace BioDomes.Infrastructures.Models;

public class BiomeSpecieEntity
{
    public int Id { get; set; }
    public DateTime Inserted { get; set; }
    // relation
    public int BiomeId { get; set; }
    public BiomeEntity Biome { get; set; } = null!;
    
    public int SpecieId { get; set; }
    public SpecieEntity Specie { get; set; } = null!;
}