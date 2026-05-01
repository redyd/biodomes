namespace BioDomes.Infrastructures.Models;

public class BiomeSpecieEntity
{
    public int Count { get; set; }
    
    // relation
    public int BiomeId { get; set; }
    public BiomeEntity Biome { get; set; }
    
    public int SpecieId { get; set; }
    public SpecieEntity Specie { get; set; }
}