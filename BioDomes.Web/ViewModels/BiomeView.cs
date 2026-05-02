using BioDomes.Domains.Core;

namespace BioDomes.Web.ViewModels;

public class BiomeView
{
    public required string? ImageUrl { get; init; }
    public required bool CanEdit { get; init; }
    public required bool IsPublic { get; init; }
    public required Biome Model { get; init; }
    public double MaxHumidity { get; init; }
    public double HumidityPercent { get; init; }
}