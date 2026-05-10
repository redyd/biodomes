namespace BioDomes.Domains.Core;

public class BiomeStats
{
    public required ComplexBiome Biome { get; init; }
    public required int TotalsSpecies { get; init; }
    public bool AllSpeciesFed { get; init; }
    public IReadOnlyList<FoodChainNode> StarvedSpecies { get; init; } = [];
    public IReadOnlyList<FoodChainNode> UnderfedSpecies { get; init; } = [];
    public int ProblemCount { get; init; }
}