using BioDomes.Domains.Core;
using BioDomes.Domains.Extensions;

namespace BioDomes.Domains.Tests.Core;

[TestFixture]
public class FoodChainGraphTests
{
    // ─── Diets ───────────────────────────────────────────────────────────────
    private static readonly Diet Carnivore   = new("carnivore",   "Carnivore");
    private static readonly Diet Herbivore   = new("herbivore",   "Herbivore");
    private static readonly Diet Omnivore    = new("omnivore",    "Omnivore");
 
    // ─── Classifications ─────────────────────────────────────────────────────
    // Carnivores eat herbivores and omnivores
    private static readonly Classification PredatorClass = new(
        "predator", "Predator",
        eatenByDiets: [Carnivore, Omnivore],
        requireFood: true
    );
 
    // Herbivores eat plants (no diet listed here eats them for simplicity,
    // but carnivores prey on them → represented via PredatorClass.EatenByDiets)
    private static readonly Classification HerbivoreClass = new(
        "herbivore-class", "Herbivore Class",
        eatenByDiets: [Carnivore, Omnivore],
        requireFood: true
    );
 
    // Plants are eaten by herbivores and omnivores
    private static readonly Classification PlantClass = new(
        "plant", "Plant",
        eatenByDiets: [Herbivore, Omnivore],
        requireFood: false
    );
 
    // ─── Species helpers ─────────────────────────────────────────────────────
    private static Specie MakeSpecie(string name, Diet diet, Classification classification) => new()
    {
        Name = name,
        Height = 100,
        Weight = 50,
        ImageUrl = null,
        IsPublic = true,
        CreatedBy = "test",
        Diet = diet,
        Classification = classification,
        Slug = name.ToKebabCase(),
    };
 
    // Wolf  → carnivore, predator class  (slug: "wolf")
    // Deer  → herbivore, herbivore class (slug: "deer")
    // Grass → herbivore, plant class     (slug: "grass")
    private static readonly Specie Wolf  = MakeSpecie("Wolf",  Carnivore, PredatorClass);
    private static readonly Specie Deer  = MakeSpecie("Deer",  Herbivore, HerbivoreClass);
    private static readonly Specie Grass = MakeSpecie("Grass", Herbivore, PlantClass);
 
    private static FoodChainNode NodeOf(Specie s) => new(s, 1);
 
    // =========================================================================
    // CountRelations
    // =========================================================================
 
    [Test]
    public void CountRelations_EmptyGraph_ReturnsEmptyDictionary()
    {
        var graph = new FoodChainGraph([], []);
 
        var result = graph.CountRelations();
 
        Assert.That(result, Is.Empty);
    }
 
    [Test]
    public void CountRelations_NodeWithNoEdges_ReturnsZero()
    {
        var nodes = new[] { NodeOf(Wolf) };
        var graph = new FoodChainGraph(nodes, []);
 
        var result = graph.CountRelations();
 
        Assert.That(result[Wolf], Is.EqualTo(0));
    }
 
    [Test]
    public void CountRelations_SingleEdge_CountsOneForBothEndpoints()
    {
        // Wolf → Deer
        var nodes = new[] { NodeOf(Wolf), NodeOf(Deer) };
        var edges = new[] { new FoodChainEdge(Wolf.Slug, Deer.Slug) };
        var graph = new FoodChainGraph(nodes, edges);
 
        var result = graph.CountRelations();
 
        Assert.That(result[Wolf], Is.EqualTo(1));
        Assert.That(result[Deer], Is.EqualTo(1));
    }
 
    [Test]
    public void CountRelations_MultipleEdges_CountsAllAppearances()
    {
        // Wolf → Deer, Wolf → Grass
        var nodes = new[] { NodeOf(Wolf), NodeOf(Deer), NodeOf(Grass) };
        var edges = new[]
        {
            new FoodChainEdge(Wolf.Slug, Deer.Slug),
            new FoodChainEdge(Wolf.Slug, Grass.Slug),
        };
        var graph = new FoodChainGraph(nodes, edges);
 
        var result = graph.CountRelations();
 
        Assert.That(result[Wolf],  Is.EqualTo(2)); // appears as predator twice
        Assert.That(result[Deer],  Is.EqualTo(1));
        Assert.That(result[Grass], Is.EqualTo(1));
    }
 
    [Test]
    public void CountRelations_ContainsAllNodes_EvenWithZeroEdges()
    {
        var nodes = new[] { NodeOf(Wolf), NodeOf(Deer), NodeOf(Grass) };
        var graph = new FoodChainGraph(nodes, []);
 
        var result = graph.CountRelations();
 
        Assert.That(result.Keys, Has.Count.EqualTo(3));
    }
 
    // =========================================================================
    // PreyOf
    // =========================================================================
 
    [Test]
    public void PreyOf_UnknownPredator_ReturnsEmptyList()
    {
        var nodes = new[] { NodeOf(Wolf), NodeOf(Deer) };
        var edges = new[] { new FoodChainEdge(Wolf.Slug, Deer.Slug) };
        var graph = new FoodChainGraph(nodes, edges);
 
        var result = graph.PreyOf("unknown-slug");
 
        Assert.That(result, Is.Empty);
    }
 
    [Test]
    public void PreyOf_PredatorWithNoPrey_ReturnsEmptyList()
    {
        var nodes = new[] { NodeOf(Wolf), NodeOf(Deer) };
        var graph = new FoodChainGraph(nodes, []);
 
        var result = graph.PreyOf(Wolf.Slug);
 
        Assert.That(result, Is.Empty);
    }
 
    [Test]
    public void PreyOf_ReturnsSinglePrey()
    {
        var nodes = new[] { NodeOf(Wolf), NodeOf(Deer) };
        var edges = new[] { new FoodChainEdge(Wolf.Slug, Deer.Slug) };
        var graph = new FoodChainGraph(nodes, edges);
 
        var result = graph.PreyOf(Wolf.Slug);
 
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Specie, Is.EqualTo(Deer));
    }
 
    [Test]
    public void PreyOf_ReturnsMultiplePrey()
    {
        var nodes = new[] { NodeOf(Wolf), NodeOf(Deer), NodeOf(Grass) };
        var edges = new[]
        {
            new FoodChainEdge(Wolf.Slug, Deer.Slug),
            new FoodChainEdge(Wolf.Slug, Grass.Slug),
        };
        var graph = new FoodChainGraph(nodes, edges);
 
        var result = graph.PreyOf(Wolf.Slug);
 
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Select(n => n.Specie), Does.Contain(Deer));
        Assert.That(result.Select(n => n.Specie), Does.Contain(Grass));
    }
 
    [Test]
    public void PreyOf_DoesNotReturnPredatorItself()
    {
        var nodes = new[] { NodeOf(Wolf), NodeOf(Deer) };
        var edges = new[] { new FoodChainEdge(Wolf.Slug, Deer.Slug) };
        var graph = new FoodChainGraph(nodes, edges);
 
        var result = graph.PreyOf(Wolf.Slug);
 
        Assert.That(result.Select(n => n.Specie), Does.Not.Contain(Wolf));
    }
 
    // =========================================================================
    // AvailablePreyFor
    // =========================================================================
 
    [Test]
    public void AvailablePreyFor_UnknownPredator_ReturnsEmptyList()
    {
        var nodes = new[] { NodeOf(Wolf), NodeOf(Deer) };
        var graph = new FoodChainGraph(nodes, []);
 
        var result = graph.AvailablePreyFor("unknown-slug");
 
        Assert.That(result, Is.Empty);
    }
 
    [Test]
    public void AvailablePreyFor_ExcludesPredatorItself()
    {
        // Wolf is carnivore; PredatorClass has no eatenByDiets that match Wolf's
        // diet, so Wolf cannot eat itself — but we verify the slug exclusion rule.
        var nodes = new[] { NodeOf(Wolf) };
        var graph = new FoodChainGraph(nodes, []);
 
        var result = graph.AvailablePreyFor(Wolf.Slug);
 
        Assert.That(result.Select(n => n.Specie), Does.Not.Contain(Wolf));
    }
 
    [Test]
    public void AvailablePreyFor_ExcludesAlreadyConnectedPrey()
    {
        // Wolf already eats Deer → Deer must not appear as available
        var nodes = new[] { NodeOf(Wolf), NodeOf(Deer), NodeOf(Grass) };
        var edges = new[] { new FoodChainEdge(Wolf.Slug, Deer.Slug) };
        var graph = new FoodChainGraph(nodes, edges);
 
        var result = graph.AvailablePreyFor(Wolf.Slug);
 
        Assert.That(result.Select(n => n.Specie), Does.Not.Contain(Deer));
    }
 
    [Test]
    public void AvailablePreyFor_ReturnsCompatibleUnconnectedPrey()
    {
        // Wolf is carnivore. HerbivoreClass.EatenByDiets contains Carnivore
        // → Deer (herbivore class) should be available.
        var nodes = new[] { NodeOf(Wolf), NodeOf(Deer) };
        var graph = new FoodChainGraph(nodes, []);
 
        var result = graph.AvailablePreyFor(Wolf.Slug);
 
        Assert.That(result.Select(n => n.Specie), Does.Contain(Deer));
    }
 
    [Test]
    public void AvailablePreyFor_ExcludesIncompatibleClassification()
    {
        // PlantClass.EatenByDiets = [Herbivore, Omnivore] — does NOT include Carnivore.
        // Wolf is carnivore → Grass (plant class) should NOT be available.
        var nodes = new[] { NodeOf(Wolf), NodeOf(Grass) };
        var graph = new FoodChainGraph(nodes, []);
 
        var result = graph.AvailablePreyFor(Wolf.Slug);
 
        Assert.That(result.Select(n => n.Specie), Does.Not.Contain(Grass));
    }
 
    [Test]
    public void AvailablePreyFor_EmptyGraph_ReturnsEmptyList()
    {
        var graph = new FoodChainGraph([], []);
 
        var result = graph.AvailablePreyFor("any-slug");
 
        Assert.That(result, Is.Empty);
    }
}