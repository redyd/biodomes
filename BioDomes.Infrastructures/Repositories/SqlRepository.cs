using BioDomes.Domains.Core;
using BioDomes.Domains.Extensions;
using BioDomes.Domains.Repository;

namespace BioDomes.Infrastructures.Repositories;

public class SqlRepository : IRepository
{
    
    private readonly List<Species> _species;

    public SqlRepository()
    {
        _species = new List<Species>
        {
            new Species("Lion", "Mammifère", "Carnivore", 120),
            new Species("Vache", "Mammifère", "Herbivore", 150),
            new Species("Aigle", "Oiseau", "Carnivore", 70),
            new Species("Chêne", "Plante", "Photosynthèse", 2000),
            new Species("Renard", "Mammifère", "Omnivore", 40),
            new Species("Requin", "Poisson", "Carnivore", 500),
            new Species("Bambou", "Plante", "Photosynthèse", 1000),
            new Species("Ours", "Mammifère", "Omnivore", 180),
            new Species("Poulet", "Oiseau", "Omnivore", 45),
            new Species("Lapin", "Mammifère", "Herbivore", 35)
        };
    }
    
    public List<Species> FindAll()
    {
        return _species;
    }

    public Species? FindBySlug(string slug)
    {
        return _species.FirstOrDefault(s => s.Name.ToKebabCase() == slug.ToKebabCase());
    }

    public Species? Update(string slug, Species input)
    {
        if (_species.Any(s => s.Name.ToLower() == input.Name.ToLower()))
            throw new InvalidOperationException($"Une espèce nommée '{input.Name}' existe déjà.");
        
        var species = _species.FirstOrDefault(s => s.Name.ToKebabCase() == slug);
        if (species == null) return null;

        species.Name = input.Name;
        species.Type = input.Type;
        species.Diet = input.Diet;
        species.Height = input.Height;
        species.ImageUrl = input.ImageUrl;

        return species;
    }

    public Species Add(Species data)
    {
        if (_species.Any(s => s.Name.ToLower() == data.Name.ToLower()))
            throw new InvalidOperationException($"Une espèce nommée '{data.Name}' existe déjà.");

        _species.Add(data);
        return data;
    }

    public List<Species> Delete(string name)
    {
        var specieToDelete = FindBySlug(name);

        if (specieToDelete == null)
        {
            return _species;
        }
        
        _species.Remove(specieToDelete);
        return _species;
    }
}