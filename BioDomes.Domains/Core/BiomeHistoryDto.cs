namespace BioDomes.Domains.Core;

public class BiomeHistoryDto
{
    public int BiomeId { get; set; }
    public string BiomeName { get; set; } = string.Empty;
    
    public List<HistoryPoint> SpeciesOverTime { get; set; } = new();
    
    public List<HistoryPoint> ByDietOverTime { get; set; } = new();
    public List<HistoryPoint> ByClassificationOverTime { get; set; } = new();
}