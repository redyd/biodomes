namespace BioDomes.Domains.Core;

public class Biome
{
    public required string CreatedBy { get; set; }
    public required int Id { get; set; }
    public string Name { get; set; }
    public double? Temperature { get; set; }
    public double? Humidity { get; set; }
    public required string? ImageUrl { get; set; }
    public bool IsPublic { get; set; }
    
    public string State => Temperature switch
    {
        < -20 or > 40 => "Critique",
        < 0 or > 30   => "Instable",
        _             => "Optimal"
    };
}