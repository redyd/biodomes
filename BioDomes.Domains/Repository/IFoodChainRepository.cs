using BioDomes.Domains.Core;

namespace BioDomes.Domains.Repository;

public interface IFoodChainRepository
{
    Task<Result<FoodChainGraph>> GetFullGraphForBiomeAsync(int biomeId, string? userId, bool isAdmin);
    Task<Result<FoodChainGraph>> GetFullGraphAsync(string? userId, bool isAdmin);

    Task DeleteRelation(int predatorId, int preyId);
    Task AddRelation(int predatorId, int preyId);
}
