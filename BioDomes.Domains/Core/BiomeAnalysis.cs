namespace BioDomes.Domains.Core;

public class BiomeAnalysis
{
    public required FoodChainGraph Graph { get; init; }
    public required BiomeStats Stats { get; init; }
}