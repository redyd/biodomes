using BioDomes.Domains.Core;

namespace BioDomes.Domains.Repository;

public interface IFoodChainRepository
{
    Task<FoodChainGraph> GetFullGraphAsync();
    Task DeleteRelation(string predatorSlug, string preySlug);
    Task AddRelation(string predatorSlug, string preySlug);
}
