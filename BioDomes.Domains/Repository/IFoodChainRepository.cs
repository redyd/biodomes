using BioDomes.Domains.Core;

namespace BioDomes.Domains.Repository;

public interface IFoodChainRepository
{
    Task<FoodChainGraph> GetFullGraphAsync();
}
