
namespace BioDomes.Domains.Core;

public class Specie
{
    public required string Slug { get; init; }
    public required string Name { get; init; }
    public required int Height { get; init; }
    public required int Weight { get; init; }
    public required string? ImageUrl { get; init; }
    public required bool IsPublic { get; init; }
    public required string CreatedBy { get; init; }

    public required Diet Diet { get; init; }
    public required Classification Classification { get; init; }
}