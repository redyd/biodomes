using BioDomes.Domains.Core;

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
}