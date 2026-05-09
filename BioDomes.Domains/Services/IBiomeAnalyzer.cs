using BioDomes.Domains.Core;

namespace BioDomes.Domains.Services;

public interface IBiomeAnalyzer
{
    Task<List<Recommendation<Specie>>> RecommendationToAddForBiome(string? userId, bool isAdmin, int biomeId,
        FoodChainGraph biomeGraph);

    Task<BiomeAnalysis> AnalyzeBiome(string? userId, bool isAdmin, int biomeId);
    Task<Dictionary<ComplexBiome, BiomeStats>> AnalyzeAllBiomes(string? userId, bool isAdmin);
    Task<GlobalStatistic> GlobalAnalyze(string? userId, bool isAdmin);

    Task<Result> PerformAction(string actionId, string? userId, bool isAdmin);
}