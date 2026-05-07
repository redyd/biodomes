using BioDomes.Domains.Core;

namespace BioDomes.Domains.Repository;

public interface IFoodChainRepository
{
    Task<ResultT<FoodChainGraph>> GetFullGraphForBiomeAsync(int biomeId, string? userId, bool isAdmin);
    Task<ResultT<FoodChainGraph>> GetFullGraphAsync(string? userId, bool isAdmin);

    Task<Result> DeleteRelation(int predatorId, int preyId);
    Task<Result> AddRelation(int predatorId, int preyId);
}
