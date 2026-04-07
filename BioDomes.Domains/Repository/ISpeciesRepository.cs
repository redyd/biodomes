using BioDomes.Domains.Core;

namespace BioDomes.Domains.Repository;

public interface ISpeciesRepository
{
    Task<List<Specie>> FindAllAsync();

    Task<int> CountAsync();

    Task<Specie?> FindBySlugAsync(string slug);

    Task<Specie?> UpdateAsync(string previousSlug, Specie toUpdate);

    Task<Specie> AddAsync(Specie toAdd);
    
    Task<Specie?> DeleteAsync(string slug);
}