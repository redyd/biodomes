using BioDomes.Domains.Core;

namespace BioDomes.Domains.Repository;

public interface ISpecieAttributesRepository
{
    Task<ResultT<List<Classification>>> GetClassifications();
    Task<ResultT<List<Diet>>> GetDiets();
}