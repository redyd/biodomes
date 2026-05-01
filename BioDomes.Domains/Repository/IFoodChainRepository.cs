using BioDomes.Domains.Core;

namespace BioDomes.Domains.Repository;

public interface IFoodChainRepository
{
    Task<FoodChainGraph> GetFullGraphAsync();
    Task<Result<FoodChainGraph>> GetFullGraphForBiomeAsync(int biomeId);
    Task DeleteRelation(int predatorId, int preyId);
    Task AddRelation(int predatorId, int preyId);
}
