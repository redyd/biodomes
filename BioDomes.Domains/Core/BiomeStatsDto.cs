namespace BioDomes.Domains.Core;

public class BiomeStatsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int SpeciesCount { get; set; }
    public int EquipmentCount { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public Dictionary<string, int> SpeciesByDiet { get; set; } = new();
    public Dictionary<string, int> SpeciesByClassification { get; set; } = new();
}