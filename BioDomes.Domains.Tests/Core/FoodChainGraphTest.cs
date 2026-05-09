using BioDomes.Domains.Core;

namespace BioDomes.Domains.Tests.Core;

[TestFixture]
public class FoodChainGraphTests
{
    private FoodChainNode CreateNode(
        int specieId,
        Diet diet,
        Classification classification,
        int count = 1)
    {
        return new FoodChainNode(
            specieId,
            $"Specie {specieId}",
            diet,
            classification,
            count
        );
    }

    private FoodChainEdge CreateEdge(int predatorId, int preyId)
    {
        return new FoodChainEdge(predatorId, preyId);
    }

    [Test]
    public void should_return_species_without_prey()
    {
        var diet = new Diet(1, "Carnivore");

        var classification = new Classification(
            1,
            "Mammal",
            new List<Diet>(),
            true
        );

        var node1 = CreateNode(1, diet, classification);
        var node2 = CreateNode(2, diet, classification);
        var node3 = CreateNode(3, diet, classification);

        var graph = new FoodChainGraph(
            new List<FoodChainNode> { node1, node2, node3 },
            new List<FoodChainEdge>
            {
                CreateEdge(1, 2)
            }
        );

        var result = graph.NoPreySpecies();

        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result.Contains(node2));
        Assert.That(result.Contains(node3));
    }

    [Test]
    public void should_count_all_relations_for_each_species()
    {
        var diet = new Diet(1, "Carnivore");

        var classification = new Classification(
            1,
            "Mammal",
            new List<Diet>(),
            true
        );

        var node1 = CreateNode(1, diet, classification);
        var node2 = CreateNode(2, diet, classification);
        var node3 = CreateNode(3, diet, classification);

        var graph = new FoodChainGraph(
            new List<FoodChainNode> { node1, node2, node3 },
            new List<FoodChainEdge>
            {
                CreateEdge(1, 2),
                CreateEdge(1, 3)
            }
        );

        var result = graph.CountRelations();

        Assert.That(result[1], Is.EqualTo(2));
        Assert.That(result[2], Is.EqualTo(1));
        Assert.That(result[3], Is.EqualTo(1));
    }

    [Test]
    public void should_count_prey_relations_for_each_species()
    {
        var diet = new Diet(1, "Carnivore");

        var classification = new Classification(
            1,
            "Mammal",
            new List<Diet>(),
            true
        );

        var node1 = CreateNode(1, diet, classification);
        var node2 = CreateNode(2, diet, classification);
        var node3 = CreateNode(3, diet, classification);

        var graph = new FoodChainGraph(
            new List<FoodChainNode> { node1, node2, node3 },
            new List<FoodChainEdge>
            {
                CreateEdge(1, 2),
                CreateEdge(1, 3)
            }
        );

        var result = graph.CountPreyRelations();

        Assert.That(result[1], Is.EqualTo(2));
        Assert.That(result[2], Is.EqualTo(0));
        Assert.That(result[3], Is.EqualTo(0));
    }

    [Test]
    public void should_return_prey_of_predator()
    {
        var diet = new Diet(1, "Carnivore");

        var classification = new Classification(
            1,
            "Mammal",
            new List<Diet>(),
            true
        );

        var node1 = CreateNode(1, diet, classification);
        var node2 = CreateNode(2, diet, classification);
        var node3 = CreateNode(3, diet, classification);

        var graph = new FoodChainGraph(
            new List<FoodChainNode> { node1, node2, node3 },
            new List<FoodChainEdge>
            {
                CreateEdge(1, 2),
                CreateEdge(1, 3)
            }
        );

        var result = graph.PreyOf(1);

        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result.Contains(node2));
        Assert.That(result.Contains(node3));
    }

    [Test]
    public void should_return_empty_list_when_predator_does_not_exist()
    {
        var graph = new FoodChainGraph(
            new List<FoodChainNode>(),
            new List<FoodChainEdge>()
        );

        var result = graph.AvailablePreyFor(999);

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void should_return_available_prey_for_predator()
    {
        var carnivore = new Diet(1, "Carnivore");
        var herbivore = new Diet(2, "Herbivore");

        var predatorClassification = new Classification(
            1,
            "Predator",
            new List<Diet>(),
            true
        );

        var preyClassification = new Classification(
            2,
            "Prey",
            new List<Diet> { carnivore },
            true
        );

        var predator = CreateNode(1, carnivore, predatorClassification);

        var prey1 = CreateNode(2, herbivore, preyClassification);
        var prey2 = CreateNode(3, herbivore, preyClassification);

        var graph = new FoodChainGraph(
            new List<FoodChainNode> { predator, prey1, prey2 },
            new List<FoodChainEdge>
            {
                CreateEdge(1, 2)
            }
        );

        var result = graph.AvailablePreyFor(1);

        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result.Contains(prey2));
    }
}