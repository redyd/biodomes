using BioDomes.Domains.Core;

namespace BioDomes.Web.Dtos;

public record NodeClickedDetailDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required Classification Classification { get; init; }
    public required IReadOnlyList<FoodChainNode> Preys { get; init; }
    public required IReadOnlyList<FoodChainNode> Available { get; init; }
}