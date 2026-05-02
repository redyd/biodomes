namespace BioDomes.Domains.Core;

public class BiomeStats
{
    public bool AllSpeciesFed { get; init; }
    public IReadOnlyList<FoodChainNode> StarvedSpecies { get; init; } = [];
    public IReadOnlyList<FoodChainNode> UnderfedSpecies { get; init; } = [];

    public int ProblemCount => StarvedSpecies
        .Select(s => s.SpecieId)
        .Concat(UnderfedSpecies.Select(s => s.SpecieId))
        .Distinct()
        .Count();
}