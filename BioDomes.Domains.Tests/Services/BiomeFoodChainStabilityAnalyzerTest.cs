using BioDomes.Domains.Core;
using BioDomes.Domains.Services;

namespace BioDomes.Domains.Tests.Services;

[TestFixture]
public class BiomeFoodChainStabilityAnalyzerTests
{
    private Specie CreateSpecie(int id, int weight = 1)
    {
        return new Specie
        {
            Slug = $"s{id}",
            Name = $"S{id}",
            Height = 1,
            Weight = weight,
            ImageUrl = null,
            IsPublic = true,
            CreatedBy = "test",
            Diet = new Diet("1", "Carnivore"),
            Classification = new Classification("1", "A", new List<Diet>(), true)
        };
    }

    private FoodChainNode CreateNode(int id, int count = 1)
    {
        return new FoodChainNode(
            new Specie
            {
                Slug = $"{id}",
                Name = $"{id}",
                Height = 67,
                Weight = 67,
                Classification = new Classification("1",
                    "A",
                    new List<Diet>(),
                    true),
                Diet = new Diet("1",
                    "Carnivore"),
                CreatedBy = "test",
                IsPublic = true,
                ImageUrl = null
            },
            count
        );
    }

    [Test]
    public void should_return_empty_starved_when_all_species_have_preys()
    {
        var graph = new FoodChainGraph(
            new List<FoodChainNode>
            {
                CreateNode(1),
                CreateNode(2)
            },
            new List<FoodChainEdge>
            {
                new FoodChainEdge("1", "2"),
                new FoodChainEdge("2", "1")
            }
        );

        var species = new Dictionary<int, Specie>
        {
            { 1, CreateSpecie(1) },
            { 2, CreateSpecie(2) }
        };

        var result = BiomeFoodChainStabilityAnalyzer.Analyze(graph, species, 1);

        Assert.That(result.StarvedSpecies, Is.Empty);
    }

    [Test]
    public void should_detect_starved_species_when_no_edges_exist()
    {
        var graph = new FoodChainGraph(
            new List<FoodChainNode>
            {
                CreateNode(1),
                CreateNode(2)
            },
            new List<FoodChainEdge>()
        );

        var species = new Dictionary<int, Specie>
        {
            { 1, CreateSpecie(1) },
            { 2, CreateSpecie(2) }
        };

        var result = BiomeFoodChainStabilityAnalyzer.Analyze(graph, species, 1);

        Assert.That(result.StarvedSpecies.Count, Is.EqualTo(2));
    }

    [Test]
    public void should_return_underfed_when_biomass_is_insufficient()
    {
        var predator = CreateSpecie(1, weight: 10);
        var prey = CreateSpecie(2, weight: 1);

        var graph = new FoodChainGraph(
            new List<FoodChainNode>
            {
                CreateNode(1, count: 1),
                CreateNode(2, count: 1)
            },
            new List<FoodChainEdge>
            {
                new FoodChainEdge(1, 2)
            }
        );

        var species = new Dictionary<int, Specie>
        {
            { 1, predator },
            { 2, prey }
        };

        var result = BiomeFoodChainStabilityAnalyzer.Analyze(graph, species, 1);

        Assert.That(result.UnderfedSpecies, Is.Not.Empty);
    }

    [Test]
    public void should_detect_loop_when_mutual_predation_exists()
    {
        var graph = new FoodChainGraph(
            new List<FoodChainNode>
            {
                CreateNode(1),
                CreateNode(2)
            },
            new List<FoodChainEdge>
            {
                new FoodChainEdge(1, 2),
                new FoodChainEdge(2, 1)
            }
        );

        var species = new Dictionary<int, Specie>
        {
            { 1, CreateSpecie(1) },
            { 2, CreateSpecie(2) }
        };

        var candidate = CreateSpecie(3);

        var result = BiomeFoodChainStabilityAnalyzer.IntroducesLoop(graph, candidate,
            new FoodChainGraph(
                new List<FoodChainNode>(),
                new List<FoodChainEdge>()
            ));

        Assert.That(result, Is.False);
    }

    [Test]
    public void should_return_stable_when_no_underfed_species_are_added()
    {
        var graph = new FoodChainGraph(
            new List<FoodChainNode>
            {
                CreateNode(1)
            },
            new List<FoodChainEdge>()
        );

        var species = new Dictionary<int, Specie>
        {
            { 1, CreateSpecie(1) }
        };

        var candidate = CreateSpecie(2);

        var global = new FoodChainGraph(
            new List<FoodChainNode>(),
            new List<FoodChainEdge>()
        );

        var result = BiomeFoodChainStabilityAnalyzer.IsStableAfterAdding(
            1,
            graph,
            candidate,
            global,
            species
        );

        Assert.That(result, Is.True);
    }
}