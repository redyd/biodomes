namespace BioDomes.Domains.Core;

public class BiomeStats
{
    public bool AllSpeciesFed { get; init; }
    public IReadOnlyList<FoodChainNode> StarvedSpecies { get; init; } = [];
    public IReadOnlyList<FoodChainNode> UnderfedSpecies { get; init; } = [];

    public int ProblemCount { get; init; }
}