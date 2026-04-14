using BioDomes.Domains.Core;
using BioDomes.Domains.Requests;

namespace BioDomes.Domains.Repository;

public interface ISpeciesRepository
{
    Task<List<Specie>> FindAllAsync(string userId, bool isAdmin);

    Task<int> CountAsync();

    Task<Result<Specie>> FindBySlugAsync(string slug, string userId, bool isAdmin);

    Task<Specie?> UpdateAsync(string previousSlug, Specie toUpdate, string userId);

    Task<Result<Specie>> AddAsync(CreateSpecieRequest toAdd);
    
    Task<Specie?> DeleteAsync(string slug);
}