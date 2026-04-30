namespace BioDomes.Web.ViewModels;

public class BiomeView
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public double MaxHumidity { get; set; }
    public double HumidityPercent { get; set; }
    public required string? ImageUrl { get; set; }
    public required bool CanEdit { get; set; }
    public required bool IsPublic { get; set; }
    
    public required string State { get; set; }
}