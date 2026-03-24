using BioDomes.Domains.Core;
using BioDomes.Domains.Extensions;

namespace BioDomes.Infrastructures.repository;

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

    public Species? Update(string slug, SpeciesData input)
    {
        if (_species.Any(s => s.Name.ToLower() == input.Name.ToLower()))
            throw new InvalidOperationException($"Une espèce nommée '{input.Name}' existe déjà.");
        
        var species = _species.FirstOrDefault(s => s.Name.ToKebabCase() == slug);
        if (species == null) return null;

        species.Name = input.Name;
        species.Type = input.Type;
        species.Diet = input.Diet;
        species.Height = input.Height;

        return species;
    }
    
    public void UpdateImage(string name, string imagePath)
    {
        var species = FindBySlug(name);
    
        if (species == null) return;
    
        species.ImageUrl = imagePath;
    }

    public Species Add(SpeciesData data)
    {
        if (_species.Any(s => s.Name.ToLower() == data.Name.ToLower()))
            throw new InvalidOperationException($"Une espèce nommée '{data.Name}' existe déjà.");

        var species = new Species(data.Name, data.Type, data.Diet, data.Height);
        _species.Add(species);
        return species;
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