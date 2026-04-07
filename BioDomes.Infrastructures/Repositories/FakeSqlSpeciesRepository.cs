using BioDomes.Domains.Core;
using BioDomes.Domains.Extensions;
using BioDomes.Domains.Repository;

namespace BioDomes.Infrastructures.Repositories;

public class FakeSqlSpeciesRepository : ISpeciesRepository
{
    
    private readonly List<Specie> _species;

    public FakeSqlSpeciesRepository()
    {
        _species = new List<Specie>
        {
            new Specie("Lion", "Mammifère", "Carnivore", 120, 50),
            new Specie("Vache", "Mammifère", "Herbivore", 150, 50),
            new Specie("Aigle", "Oiseau", "Carnivore", 70, 50),
            new Specie("Chêne", "Plante", "Photosynthèse", 2000, 50),
            new Specie("Renard", "Mammifère", "Omnivore", 40, 50),
            new Specie("Requin", "Poisson", "Carnivore", 500, 50),
            new Specie("Bambou", "Plante", "Photosynthèse", 1000, 50),
            new Specie("Ours", "Mammifère", "Omnivore", 180, 50),
            new Specie("Poulet", "Oiseau", "Omnivore", 45, 50),
            new Specie("Lapin", "Mammifère", "Herbivore", 35, 50)
        };
    }
    
    public List<Specie> FindAll()
    {
        return _species;
    }

    public Specie? FindBySlug(string slug)
    {
        return _species.FirstOrDefault(s => s.Name.ToKebabCase() == slug.ToKebabCase());
    }

    public Specie? Update(string previousSlug, Specie toUpdate)
    {
        if (_species.Any(s => s.Name.ToLower() == toUpdate.Name.ToLower()))
            throw new InvalidOperationException($"Une espèce nommée '{toUpdate.Name}' existe déjà.");
        
        var species = _species.FirstOrDefault(s => s.Name.ToKebabCase() == previousSlug);
        if (species == null) return null;

        species.Name = toUpdate.Name;
        species.Type = toUpdate.Type;
        species.Diet = toUpdate.Diet;
        species.Height = toUpdate.Height;
        species.Weight = toUpdate.Weight;
        species.ImageUrl = toUpdate.ImageUrl;

        return species;
    }

    public Specie Add(Specie toAdd)
    {
        if (_species.Any(s => s.Name.ToLower() == toAdd.Name.ToLower()))
            throw new InvalidOperationException($"Une espèce nommée '{toAdd.Name}' existe déjà.");

        _species.Add(toAdd);
        return toAdd;
    }

    public Specie? Delete(string name)
    {
        var specieToDelete = FindBySlug(name);

        if (specieToDelete == null)
        {
            return null;
        }
        
        _species.Remove(specieToDelete);
        return specieToDelete;
    }
}
