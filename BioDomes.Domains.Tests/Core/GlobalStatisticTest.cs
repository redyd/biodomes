using BioDomes.Domains.Core;

namespace BioDomes.Domains.Tests.Core;

[TestFixture]
public class GlobalStatisticTests
{
    private Specie CreateSpecie(int id)
    {
        return new Specie
        {
            Slug = $"specie-{id}",
            Name = $"Specie {id}",
            Height = 10,
            Weight = 10,
            ImageUrl = null,
            IsPublic = true,
            CreatedBy = "test",
            Diet = new Diet("1", "Carnivore"),
            Classification = new Classification(
                "1",
                "Mammal",
                new List<Diet>(),
                true
            )
        };
    }

    private ComplexBiome CreateBiome(BiomeState state, int speciesVariety)
    {
        var species = Enumerable.Range(0, speciesVariety)
            .Select(CreateSpecie)
            .Select(s => new Tracked<Specie>
            {
                Model = s,
                TrackedInsertionsById = new Dictionary<int, DateTime>()
            })
            .ToList();

        return new ComplexBiome
        {
            Slug = "biome",
            CreatedBy = "test",
            Name = "Biome",
            Temperature = 20,
            Humidity = 50,
            ImageUrl = null,
            IsPublic = true,
            Species = species,
            Equipments = new List<Tracked<Equipment>>()
        };
    }

    [Test]
    public void should_order_by_species_variety_descending()
    {
        var low = CreateBiome(BiomeState.Optimal, 2);
        var medium = CreateBiome(BiomeState.Optimal, 5);
        var high = CreateBiome(BiomeState.Optimal, 10);

        var stat = new GlobalStatistic
        {
            All = new List<ComplexBiome> { low, high, medium }
        };

        var result = stat.GetBalancedBiome(3);

        Assert.That(result[0].SpeciesVariety, Is.EqualTo(10));
        Assert.That(result[1].SpeciesVariety, Is.EqualTo(5));
        Assert.That(result[2].SpeciesVariety, Is.EqualTo(2));
    }

    [Test]
    public void should_take_only_requested_count()
    {
        var stat = new GlobalStatistic
        {
            All = new List<ComplexBiome>
            {
                CreateBiome(BiomeState.Optimal, 1),
                CreateBiome(BiomeState.Optimal, 2),
                CreateBiome(BiomeState.Optimal, 3)
            }
        };

        var result = stat.GetBalancedBiome(2);

        Assert.That(result.Count, Is.EqualTo(2));
    }
}