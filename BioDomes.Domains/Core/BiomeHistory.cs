namespace BioDomes.Domains.Core;

public class BiomeHistory
{
    public required string BiomeSlug { get; set; }
    public required string BiomeName { get; set; } = string.Empty;
    
    public List<HistoryPoint> SpeciesOverTime { get; set; } = new();
    
    public List<HistoryPoint> ByDietOverTime { get; set; } = new();
    public List<HistoryPoint> ByClassificationOverTime { get; set; } = new();
}