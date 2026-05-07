namespace BioDomes.Domains.Core;

public class Biome
{
    public required int Id { get; init; }
    public required string CreatedBy { get; init; }
    public required string Name { get; init; }
    public required double Temperature { get; init; }
    public required double Humidity { get; init; }
    public required string? ImageUrl { get; init; }
    public required bool IsPublic { get; init; }

    public BiomeState State => Temperature switch
    {
        < -20 or > 40 => BiomeState.Critic,
        < 0 or > 30 => BiomeState.Instable,
        _ => BiomeState.Optimal
    };
}