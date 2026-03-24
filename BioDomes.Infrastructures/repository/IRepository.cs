using BioDomes.Domains.Core;

namespace BioDomes.Infrastructures.repository;

public interface IRepository
{
    List<Species> FindAll();

    Species? FindBySlug(string slug);

    Species? Update(string slug, SpeciesData input);
    
    void UpdateImage(string slug, string imagePath);
    
    Species Add(SpeciesData data);
    
    List<Species> Delete(string slug);
}