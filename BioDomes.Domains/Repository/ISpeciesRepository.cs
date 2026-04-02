using BioDomes.Domains.Core;

namespace BioDomes.Domains.Repository;

public interface ISpeciesRepository
{
    List<Specie> FindAll();

    Specie? FindBySlug(string slug);

    Specie? Update(string previousSlug, Specie toUpdate);

    Specie Add(Specie toAdd);
    
    Specie? Delete(string slug);
}