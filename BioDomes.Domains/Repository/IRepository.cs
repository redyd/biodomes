using BioDomes.Domains.Core;

namespace BioDomes.Domains.Repository;

public interface IRepository
{
    List<Species> FindAll();

    Species? FindBySlug(string slug);

    Species? Update(string slug, Species input);
    
    void UpdateImage(string slug, string imagePath);
    
    Species Add(Species data);
    
    List<Species> Delete(string slug);
}