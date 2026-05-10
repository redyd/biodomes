using BioDomes.Domains.Core;
using BioDomes.Domains.Services;

namespace BioDomes.Domains.Tests.Services;

[TestFixture]
public class BiomeFoodChainStabilityAnalyzerTests
{
    private Specie CreateSpecie(string slug, int weight, bool requireFood = true)
    {
        return new Specie
        {
            Slug = slug,
            Name = slug,
            Height = 1,
            Weight = weight,
            ImageUrl = null,
            IsPublic = true,
            CreatedBy = "test",
            Diet = new Diet("1", "Carnivore"),
            Classification = new Classification(
                "1",
                "Animal",
                new List<Diet>(),
                requireFood
            )
        };
    }

    private FoodChainNode CreateNode(Specie specie, int count)
    {
        return new FoodChainNode(specie, count);
    }

    private ComplexBiome CreateBiome()
    {
        return new ComplexBiome
        {
            Slug = "biome",
            Name = "Biome",
            CreatedBy = "test",
            Temperature = 20,
            Humidity = 50,
            ImageUrl = null,
            IsPublic = true,
            Species = new List<Tracked<Specie>>(),
            Equipments = new List<Tracked<Equipment>>()
        };
    }

    [Test]
    public void should_return_empty_starved_when_all_species_have_preys()
    {
        var wolf = CreateSpecie("wolf", 10);
        var rabbit = CreateSpecie("rabbit", 5, false);

        var graph = new FoodChainGraph(
            new List<FoodChainNode>
            {
                CreateNode(wolf, 1),
                CreateNode(rabbit, 3)
            },
            new List<FoodChainEdge>
            {
                new("wolf", "rabbit")
            }
        );

        var species = new Dictionary<string, Specie>
        {
            { wolf.Slug, wolf },
            { rabbit.Slug, rabbit }
        };

        var result = BiomeFoodChainStabilityAnalyzer.Analyze(
            CreateBiome(),
            graph,
            species
        );

        Assert.That(result.StarvedSpecies, Is.Empty);
    }

    [Test]
    public void should_detect_starved_species_when_no_edges_exist()
    {
        var wolf = CreateSpecie("wolf", 10);
        var rabbit = CreateSpecie("rabbit", 5);

        var graph = new FoodChainGraph(
            new List<FoodChainNode>
            {
                CreateNode(wolf, 1),
                CreateNode(rabbit, 1)
            },
            new List<FoodChainEdge>()
        );

        var species = new Dictionary<string, Specie>
        {
            { wolf.Slug, wolf },
            { rabbit.Slug, rabbit }
        };

        var result = BiomeFoodChainStabilityAnalyzer.Analyze(
            CreateBiome(),
            graph,
            species
        );

        Assert.That(result.StarvedSpecies.Count, Is.EqualTo(2));
    }

    [Test]
    public void should_return_underfed_when_biomass_is_insufficient()
    {
        var wolf = CreateSpecie("wolf", 10);
        var rabbit = CreateSpecie("rabbit", 1);

        var graph = new FoodChainGraph(
            new List<FoodChainNode>
            {
                CreateNode(wolf, 1),
                CreateNode(rabbit, 1)
            },
            new List<FoodChainEdge>
            {
                new("wolf", "rabbit")
            }
        );

        var species = new Dictionary<string, Specie>
        {
            { wolf.Slug, wolf },
            { rabbit.Slug, rabbit }
        };

        var result = BiomeFoodChainStabilityAnalyzer.Analyze(
            CreateBiome(),
            graph,
            species
        );

        Assert.That(result.UnderfedSpecies, Is.Not.Empty);
    }

    [Test]
    public void should_return_all_species_fed_when_every_predator_has_enough_food()
    {
        var wolf = CreateSpecie("wolf", 10);
        var rabbit = CreateSpecie("rabbit", 5, false);

        var graph = new FoodChainGraph(
            new List<FoodChainNode>
            {
                CreateNode(wolf, 1),
                CreateNode(rabbit, 3)
            },
            new List<FoodChainEdge>
            {
                new("wolf", "rabbit")
            }
        );

        var species = new Dictionary<string, Specie>
        {
            { wolf.Slug, wolf },
            { rabbit.Slug, rabbit }
        };

        var result = BiomeFoodChainStabilityAnalyzer.Analyze(
            CreateBiome(),
            graph,
            species
        );

        Assert.That(result.AllSpeciesFed, Is.True);
    }
}