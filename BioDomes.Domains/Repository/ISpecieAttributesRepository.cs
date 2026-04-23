using BioDomes.Domains.Core;

namespace BioDomes.Domains.Repository;

public interface ISpecieAttributesRepository
{
    Task<List<Classification>> GetClassifications();
    Task<List<Diet>> GetDiets();
}