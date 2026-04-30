namespace BioDomes.Infrastructures.Models;

public class BiomeSpecieEntity
{
    public int Count { get; set; }
    
    // relation
    public int BiomeId { get; set; }
    public BiomeEntity Biome { get; set; }
    
    public string SpecieSlug { get; set; }
    public SpecieEntity Specie { get; set; }
}