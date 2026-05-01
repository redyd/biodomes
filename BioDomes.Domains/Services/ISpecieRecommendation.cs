using BioDomes.Domains.Core;

namespace BioDomes.Domains.Services;

public interface ISpecieRecommendation
{
    Task<List<Recommendation<Specie>>> RecommendationToAddForBiome(string? userId, bool isAdmin, int biomeId,
        FoodChainGraph biomeGraph);

    Task PerformAction(string actionId);
}