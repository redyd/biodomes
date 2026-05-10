namespace BioDomes.Domains.Core;

public class BiomeStatsDto
{
    public required string Slug { get; set; }
    public required string Name { get; set; } = string.Empty;
    public int SpeciesCount { get; set; }
    public int EquipmentCount { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public Dictionary<string, int> SpeciesByDiet { get; set; } = [];
    public Dictionary<string, int> SpeciesByClassification { get; set; } = [];
    public Dictionary<string, int> SpeciesByName { get; set; } = [];
}